using HabitFlow.Aplicacao.Common.Interfaces;

namespace HabitFlow.Aplicacao.Features.Habits.Commands.UpdateHabit
{
    public sealed record UpdateHabitCommand(
    Guid HabitId,
    Guid UserId,
    string Name,
    string? Description,
    string? IconName,
    string? ColorHex) : ICommand;

    public record UpdateHabitRequest(string Name, string? Description, string? IconName, string? ColorHex);
}
