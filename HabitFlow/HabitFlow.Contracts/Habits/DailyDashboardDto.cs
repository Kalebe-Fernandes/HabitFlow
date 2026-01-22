namespace HabitFlow.Contracts.Habits
{
    /// <summary>
    /// Response DTO for daily dashboard (RF-015 - Critical).
    /// Main screen showing today's progress and habits.
    /// </summary>
    public record DailyDashboardDto(
        DateTime Date,
        int CompletedHabits,
        int TotalHabits,
        decimal CompletionRate,
        int XPEarnedToday,
        int CoinsEarnedToday,
        int ActiveStreaksCount,
        DashboardHabitDto[] TodaysHabits,
        DashboardStreakDto[] TopStreaks,
        string? MotivationalMessage
    );
}
