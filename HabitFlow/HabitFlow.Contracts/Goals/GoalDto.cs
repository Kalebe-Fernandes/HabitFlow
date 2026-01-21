namespace HabitFlow.Contracts.Goals
{
    // Response DTOs
    public record GoalDto(
        Guid Id,
        string Name,
        string? Description,
        decimal TargetValue,
        string TargetUnit,
        decimal CurrentValue,
        DateTime StartDate,
        DateTime TargetDate,
        string Status, // Active, Completed, Abandoned
        decimal ProgressPercentage,
        int DaysRemaining,
        string OnTrackStatus, // "OnTrack", "Ahead", "Behind"
        List<GoalHabitDto> AssociatedHabits,
        DateTime CreatedAt
    );
}
