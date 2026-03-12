using HabitFlow.Aplicacao.Features.Habits.Commands.CompleteHabit;
using HabitFlow.Aplicacao.Features.Habits.Commands.CreateHabit;
using HabitFlow.Aplicacao.Features.Habits.Commands.UpdateHabit;
using HabitFlow.Aplicacao.Features.Habits.Queries.GetHabits;
using HabitFlow.Aplicacao.Features.Habits.Queries.GetHabitStatistics;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HabitFlow.API.Controllers
{
    [ApiController]
    [Route("api/habits")]
    [Authorize]
    public class HabitsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>Returns all active habits for the authenticated user.</summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetHabits(CancellationToken ct)
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty) return Unauthorized();

            var result = await _mediator.Send(new GetHabitsQuery(userId), ct);

            if (result.IsFailure)
                return BadRequest(new { result.Error });

            return Ok(result.Value);
        }

        /// <summary>Creates a new habit for the authenticated user.</summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CreateHabit([FromBody] CreateHabitRequest request, CancellationToken ct)
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty) return Unauthorized();

            var command = new CreateHabitCommand(
                userId,
                request.Name,
                request.Description,
                request.IconName,
                request.ColorHex,
                request.FrequencyType,
                request.TargetType,
                request.TargetValue,
                request.TargetUnit,
                request.DaysOfWeekFrequency,
                request.StartDate,
                request.EndDate,
                request.CategoryId ?? 0);

            var result = await _mediator.Send(command, ct);

            if (result.IsFailure)
                return UnprocessableEntity(new { result.Error });

            return CreatedAtAction(nameof(GetHabits), new { }, result.Value);
        }

        /// <summary>Updates display details of an existing habit.</summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateHabit(Guid id, [FromBody] UpdateHabitRequest request, CancellationToken ct)
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty) return Unauthorized();

            var command = new UpdateHabitCommand(id, userId, request.Name, request.Description, request.IconName, request.ColorHex);
            var result = await _mediator.Send(command, ct);

            if (result.IsFailure)
                return BadRequest(new { result.Error });

            return NoContent();
        }

        /// <summary>Records a completion for a habit on the specified date.</summary>
        [HttpPost("{id:guid}/complete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CompleteHabit(Guid id, [FromBody] CompleteHabitRequest request, CancellationToken ct)
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty) return Unauthorized();

            var command = new CompleteHabitCommand(
                id,
                userId,
                request.CompletionDate,
                request.CompletedValue,
                request.Notes,
                request.MoodLevel,
                request.EnergyLevel);

            var result = await _mediator.Send(command, ct);

            if (result.IsFailure)
                return BadRequest(new { result.Error });

            return Ok(result.Value);
        }

        /// <summary>Returns statistics for a specific habit.</summary>
        [HttpGet("{id:guid}/statistics")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStatistics(Guid id, CancellationToken ct)
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty) return Unauthorized();

            var result = await _mediator.Send(new GetHabitStatisticsQuery(id, userId), ct);

            if (result.IsFailure)
                return NotFound(new { result.Error });

            return Ok(result.Value);
        }

        private Guid GetCurrentUserId()
        {
            var sub = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub");

            return Guid.TryParse(sub, out var id) ? id : Guid.Empty;
        }
    }
}
