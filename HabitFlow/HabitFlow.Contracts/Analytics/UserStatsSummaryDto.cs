namespace HabitFlow.Contracts.Analytics
{
    /// <summary>
    /// Response DTO for user statistics summary.
    /// </summary>
    public record UserStatsSummaryDto(
        int TotalHabits,
        int ActiveHabits,
        int ArchivedHabits,
        int TotalCompletions,
        decimal AverageCompletionRate,
        int CurrentActiveStreaks,
        int LongestStreak,
        long TotalXPEarned,
        int CurrentLevel,
        int BadgesEarned,
        DateTime? FirstHabitDate,
        int DaysActive
    );
}
