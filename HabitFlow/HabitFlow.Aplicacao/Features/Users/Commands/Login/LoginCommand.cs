using HabitFlow.Aplicacao.Common.Interfaces;

namespace HabitFlow.Aplicacao.Features.Users.Commands.Login
{
    public record LoginCommand(string Email, string Password) : ICommand<LoginResponse>;

    public record LoginResponse(Guid UserId, string Email, string AccessToken, string RefreshToken);
}
