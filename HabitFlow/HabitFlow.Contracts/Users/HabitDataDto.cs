namespace HabitFlow.Contracts.Users
{
    /// <summary>
    /// Habit data for portability export.
    /// </summary>
    public record HabitDataDto(
        Guid Id,
        string Name,
        string? Description,
        string FrequencyType,
        string Status,
        int TotalCompletions,
        int CurrentStreak,
        int LongestStreak,
        DateTime CreatedAt
    );
}
