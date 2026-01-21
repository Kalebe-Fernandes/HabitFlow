namespace HabitFlow.Aplicacao.Features.Habits.Queries.GetHabits
{
    public record GetHabitsResponse(
        IEnumerable<HabitDto> Habits,
        int TotalCount,
        int Page,
        int PageSize
    );
}
