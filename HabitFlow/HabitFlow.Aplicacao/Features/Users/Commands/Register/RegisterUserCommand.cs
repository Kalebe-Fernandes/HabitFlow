using HabitFlow.Aplicacao.Common.Interfaces;

namespace HabitFlow.Aplicacao.Features.Users.Commands.Register
{
    public record RegisterUserCommand(
            string Email,
            string Password,
            string FirstName,
            string LastName
        ) : ICommand<RegisterUserResponse>;

    public record RegisterUserResponse(Guid UserId, string Email);
}
