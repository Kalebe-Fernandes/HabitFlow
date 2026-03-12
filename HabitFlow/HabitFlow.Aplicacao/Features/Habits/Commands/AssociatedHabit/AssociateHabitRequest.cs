namespace HabitFlow.Aplicacao.Features.Habits.Commands.AssociatedHabit
{
    public record AssociateHabitRequest(Guid HabitId, decimal ContributionWeight);
}
