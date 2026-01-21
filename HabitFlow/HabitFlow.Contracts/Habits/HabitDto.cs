namespace HabitFlow.Contracts.Habits
{
    // Response DTOs
    public record HabitDto(
        Guid Id,
        string Name,
        string? Description,
        string IconName,
        string ColorHex,
        string FrequencyType,
        int[]? DaysOfWeek,
        string TargetType,
        decimal? TargetValue,
        string? TargetUnit,
        DateTime? StartDate,
        DateTime? EndDate,
        string Status, // Active, Paused, Archived
        int CategoryId,
        string CategoryName,
        int CurrentStreak,
        int LongestStreak,
        int TotalCompletions,
        int XpReward,
        DateTime CreatedAt
    );
}
