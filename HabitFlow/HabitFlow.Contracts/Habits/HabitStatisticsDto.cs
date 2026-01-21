namespace HabitFlow.Contracts.Habits
{
    public record HabitStatisticsDto(
        Guid HabitId,
        string HabitName,
        int TotalCompletions,
        decimal CompletionRate, // Percentage
        int CurrentStreak,
        int LongestStreak,
        decimal? AverageValuePerDay, // For numeric habits
        DateTime? LastCompletionDate,
        List<DailyCompletionDto> RecentCompletions
    );
}
