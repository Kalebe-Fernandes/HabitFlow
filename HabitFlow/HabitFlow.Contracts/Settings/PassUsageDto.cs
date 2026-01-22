namespace HabitFlow.Contracts.Settings
{
    /// <summary>
    /// Pass usage history entry.
    /// </summary>
    public record PassUsageDto(
        Guid HabitId,
        string HabitName,
        DateTime Date,
        string? Reason,
        DateTime UsedAt
    );
}
