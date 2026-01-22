namespace HabitFlow.Contracts.Users
{
    /// <summary>
    /// Completion data for portability export.
    /// </summary>
    public record CompletionDataDto(
        DateTime Date,
        string HabitName,
        decimal? Value,
        string? Notes
    );
}
