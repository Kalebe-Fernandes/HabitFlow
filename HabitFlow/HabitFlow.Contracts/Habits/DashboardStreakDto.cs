namespace HabitFlow.Contracts.Habits
{
    /// <summary>
    /// Streak information for dashboard display.
    /// </summary>
    public record DashboardStreakDto(
        Guid HabitId,
        string HabitName,
        string IconName,
        int StreakDays,
        DateTime LastCompletion
    );
}
