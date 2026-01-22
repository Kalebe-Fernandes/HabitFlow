namespace HabitFlow.Contracts.Goals
{
    /// <summary>
    /// Detailed progress DTO for a goal (RF-011).
    /// </summary>
    public record GoalProgressDetailDto(
        Guid GoalId,
        string Name,
        decimal CurrentValue,
        decimal TargetValue,
        string Unit,
        decimal ProgressPercentage,
        DateTime StartDate,
        DateTime TargetDate,
        int DaysElapsed,
        int DaysRemaining,
        int TotalDays,
        string Status, // OnTrack, Ahead, Behind, Completed, Abandoned
        decimal IdealDailyProgress,
        decimal ActualDailyProgress,
        DateTime? ProjectedCompletionDate,
        GoalHabitProgressDto[] HabitContributions
    );
}
