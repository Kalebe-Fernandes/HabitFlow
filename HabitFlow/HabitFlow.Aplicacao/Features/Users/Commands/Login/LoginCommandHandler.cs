using HabitFlow.Aplicacao.Common.Interfaces;
using HabitFlow.Aplicacao.Common.Models;
using HabitFlow.Domain.Repositories;
using MediatR;

namespace HabitFlow.Aplicacao.Features.Users.Commands.Login
{
    public class LoginCommandHandler(IUserRepository userRepository, IAuthenticationService authenticationService) : IRequestHandler<LoginCommand, Result<LoginResponse>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IAuthenticationService _authenticationService = authenticationService;

        public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

            // Constant-time check avoids user enumeration via timing attacks
            if (user is null || !_authenticationService.VerifyPassword(request.Password, user.PasswordHash))
                return Result.Failure<LoginResponse>("Invalid credentials");

            if (!user.IsActive)
                return Result.Failure<LoginResponse>("Account is inactive");

            var accessToken = _authenticationService.GenerateAccessToken(user.Id, user.Email);
            var refreshToken = _authenticationService.GenerateRefreshToken();

            return Result.Success(new LoginResponse(user.Id, user.Email, accessToken, refreshToken));
        }
    }
}
