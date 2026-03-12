using HabitFlow.Aplicacao.Features.Users.Commands.Login;
using HabitFlow.Aplicacao.Features.Users.Commands.Register;
using HabitFlow.Aplicacao.Features.Users.Commands.UpdateProfile;
using HabitFlow.Aplicacao.Features.Users.Queries.GetProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HabitFlow.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>Registers a new user account.</summary>
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command, CancellationToken ct)
        {
            var result = await _mediator.Send(command, ct);

            if (result.IsFailure)
                return UnprocessableEntity(new { result.Error });

            return CreatedAtAction(nameof(GetProfile), new { }, result.Value);
        }

        /// <summary>Authenticates a user and returns JWT tokens.</summary>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken ct)
        {
            var result = await _mediator.Send(command, ct);

            if (result.IsFailure)
                return Unauthorized(new { result.Error });

            return Ok(result.Value);
        }

        /// <summary>Returns the authenticated user's profile.</summary>
        [HttpGet("profile")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProfile(CancellationToken ct)
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty) return Unauthorized();

            var result = await _mediator.Send(new GetUserProfileQuery(userId), ct);

            if (result.IsFailure)
                return NotFound(new { result.Error });

            return Ok(result.Value);
        }

        /// <summary>Updates the authenticated user's profile.</summary>
        [HttpPut("profile")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request, CancellationToken ct)
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty) return Unauthorized();

            var command = new UpdateProfileCommand(userId, request.FirstName, request.LastName, request.DisplayName);
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
