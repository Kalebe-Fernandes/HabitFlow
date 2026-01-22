namespace HabitFlow.Contracts.Habits
{
    public record DailyCompletionDto(
        DateTime Date,
        bool Completed,
        decimal? CompletedValue,
        string? Notes
    );
}
