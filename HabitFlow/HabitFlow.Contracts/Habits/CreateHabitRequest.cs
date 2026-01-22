namespace HabitFlow.Contracts.Habits
{
    // Create/Update DTOs
    public record CreateHabitRequest(
        string Name,
        string? Description,
        string IconName,
        string ColorHex,
        string FrequencyType, // "Daily", "Weekly", "Custom"
        int[]? DaysOfWeek, // 0=Sunday, 6=Saturday (for Weekly/Custom)
        string TargetType, // "Binary", "Numeric"
        decimal? TargetValue,
        string? TargetUnit,
        DateTime? StartDate,
        DateTime? EndDate,
        int CategoryId,
        int XpReward = 10
    );
}
