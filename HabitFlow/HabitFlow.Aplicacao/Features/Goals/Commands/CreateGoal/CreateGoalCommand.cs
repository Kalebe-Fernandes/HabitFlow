using HabitFlow.Aplicacao.Common.Interfaces;

namespace HabitFlow.Aplicacao.Features.Goals.Commands.CreateGoal
{
    public record CreateGoalCommand(
            Guid UserId,
            string Name,
            string? Description,
            decimal TargetValue,
            string TargetUnit,
            DateTime StartDate,
            DateTime TargetDate
        ) : ICommand<CreateGoalResponse>;

    public record CreateGoalResponse(Guid GoalId, string Name);
}
