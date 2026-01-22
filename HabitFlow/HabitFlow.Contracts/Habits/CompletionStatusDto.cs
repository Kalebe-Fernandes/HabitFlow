namespace HabitFlow.Contracts.Habits
{
    public record CompletionStatusDto(
        bool Completed,
        decimal? Value,
        bool IsSkipped // Used with "Passes" feature
    );
}
