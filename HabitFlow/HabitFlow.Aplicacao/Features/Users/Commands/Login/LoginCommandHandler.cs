using HabitFlow.Aplicacao.Common.Models;
using HabitFlow.Domain.Repositories;
using HabitFlow.Domain.Users;
using MediatR;

namespace HabitFlow.Aplicacao.Features.Users.Commands.Login
{
    public class LoginCommandHandler(IUserRepository userRepository) : IRequestHandler<LoginCommand, Result<LoginResponse>>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (user is null) return Result.Failure<LoginResponse>("Invalid credentials");

            if (!User.VerifyPassword(request.Password))
                return Result.Failure<LoginResponse>("Invalid credentials");

            // TODO: Generate JWT tokens (will be done in Infrastructure)
            var response = new LoginResponse(user.Id, user.Email, "access-token-placeholder", "refresh-token-placeholder");

            return Result.Success(response);
        }
    }
}
