namespace HabitFlow.Aplicacao.Features.Habits.Queries.GetHabitStatistics
{
    public sealed record HabitStatisticsDto(
        Guid HabitId,
        string Name,
        int CurrentStreak,
        int LongestStreak,
        int TotalCompletions,
        decimal SuccessRate,
        DateTime? LastCompletionDate,
        int DaysSinceCreation);

}
