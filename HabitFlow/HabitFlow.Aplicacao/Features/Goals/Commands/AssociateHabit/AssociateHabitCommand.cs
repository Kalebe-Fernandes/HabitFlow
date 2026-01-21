using HabitFlow.Aplicacao.Common.Interfaces;

namespace HabitFlow.Aplicacao.Features.Goals.Commands.AssociateHabit
{
    public sealed record AssociateHabitCommand(
    Guid GoalId,
    Guid HabitId,
    Guid UserId,
    decimal ContributionWeight = 1.0m) : ICommand;
}
