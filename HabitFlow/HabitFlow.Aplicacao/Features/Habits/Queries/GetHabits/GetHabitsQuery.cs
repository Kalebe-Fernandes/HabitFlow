using HabitFlow.Aplicacao.Common.Interfaces;

namespace HabitFlow.Aplicacao.Features.Habits.Queries.GetHabits
{
    public record GetHabitsQuery(
            Guid UserId,
            int Page = 1,
            int PageSize = 20,
            string? Status = "Active"
        ) : IQuery<GetHabitsResponse>;
}
