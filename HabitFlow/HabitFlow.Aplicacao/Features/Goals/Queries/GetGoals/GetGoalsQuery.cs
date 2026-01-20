using HabitFlow.Aplicacao.Common.Interfaces;

namespace HabitFlow.Aplicacao.Features.Goals.Queries.GetGoals
{
    public record GetGoalsQuery(Guid UserId) : IQuery<GetGoalsResponse>;

    public record GetGoalsResponse(IEnumerable<GoalDto> Goals);
}
