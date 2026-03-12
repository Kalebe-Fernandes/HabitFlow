using HabitFlow.Aplicacao.Features.Goals.Commands.AssociateHabit;
using HabitFlow.Aplicacao.Features.Goals.Commands.CreateGoal;
using HabitFlow.Aplicacao.Features.Goals.Queries.GetGoals;
using HabitFlow.Aplicacao.Features.Habits.Commands.AssociatedHabit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HabitFlow.API.Controllers
{
    [ApiController]
    [Route("api/goals")]
    [Authorize]
    public class GoalsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>Returns all goals for the authenticated user.</summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetGoals(CancellationToken ct)
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty) return Unauthorized();

            var result = await _mediator.Send(new GetGoalsQuery(userId), ct);

            if (result.IsFailure)
                return BadRequest(new { result.Error });

            return Ok(result.Value);
        }

        /// <summary>Creates a new long-term goal for the authenticated user.</summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CreateGoal([FromBody] CreateGoalRequest request, CancellationToken ct)
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty) return Unauthorized();

            var command = new CreateGoalCommand(
                userId,
                request.Name,
                request.Description,
                request.TargetValue,
                request.TargetUnit,
                request.StartDate,
                request.TargetDate);

            var result = await _mediator.Send(command, ct);

            if (result.IsFailure)
                return UnprocessableEntity(new { result.Error });

            return CreatedAtAction(nameof(GetGoals), new { }, result.Value);
        }

        /// <summary>Associates a habit with an existing goal.</summary>
        [HttpPost("{goalId:guid}/habits")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AssociateHabit(Guid goalId,[FromBody] AssociateHabitRequest request,CancellationToken ct)
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty) return Unauthorized();

            var command = new AssociateHabitCommand(goalId, userId, request.HabitId, request.ContributionWeight);
            var result = await _mediator.Send(command, ct);

            if (result.IsFailure)
                return BadRequest(new { result.Error });

            return NoContent();
        }

        private Guid GetCurrentUserId()
        {
            var sub = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub");

            return Guid.TryParse(sub, out var id) ? id : Guid.Empty;
        }
    }
}
