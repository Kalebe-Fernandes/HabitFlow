namespace HabitFlow.Contracts.Analytics
{
    public record WeeklyStatsDto(
        DateTime WeekStart,
        DateTime WeekEnd,
        int TotalCompletions,
        decimal AverageCompletionRate,
        int BestDay, // 0=Sunday
        int WorstDay,
        List<DailyMetricsDto> DailyBreakdown
    );
}
