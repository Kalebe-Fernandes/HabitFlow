using HabitFlow.Aplicacao.Features.Gamification.Queries.GetUserLevel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HabitFlow.API.Controllers
{
    [ApiController]
    [Route("api/gamification")]
    [Authorize]
    public class GamificationController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>Returns the XP level, progress and badge count for the authenticated user.</summary>
        [HttpGet("level")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLevel(CancellationToken ct)
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty) return Unauthorized();

            var result = await _mediator.Send(new GetUserLevelQuery(userId), ct);

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
