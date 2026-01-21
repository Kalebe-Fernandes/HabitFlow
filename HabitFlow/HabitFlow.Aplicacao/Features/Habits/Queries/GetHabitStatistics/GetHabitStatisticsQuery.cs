using HabitFlow.Aplicacao.Common.Interfaces;

namespace HabitFlow.Aplicacao.Features.Habits.Queries.GetHabitStatistics
{
    public sealed record GetHabitStatisticsQuery(Guid HabitId, Guid UserId) : IQuery<HabitStatisticsDto>;
}
