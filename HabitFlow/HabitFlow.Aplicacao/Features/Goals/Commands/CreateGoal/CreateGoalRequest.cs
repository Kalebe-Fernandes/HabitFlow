namespace HabitFlow.Aplicacao.Features.Goals.Commands.CreateGoal
{
    public record CreateGoalRequest(
            string Name,
            string? Description,
            decimal TargetValue,
            string TargetUnit,
            DateTime StartDate,
            DateTime TargetDate);
}
