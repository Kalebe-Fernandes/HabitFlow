namespace HabitFlow.Contracts.Habits
{
    /// <summary>
    /// Simplified habit DTO for dashboard display.
    /// </summary>
    public record DashboardHabitDto(
        Guid Id,
        string Name,
        string IconName,
        string ColorHex,
        bool IsCompleted,
        int CurrentStreak,
        decimal? TargetValue,
        decimal? CompletedValue,
        string? TargetUnit,
        int XpReward
    );
}
