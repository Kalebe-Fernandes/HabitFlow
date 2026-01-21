namespace HabitFlow.Contracts.Habits
{
    public record CompleteHabitRequest(
        decimal? CompletedValue, // For numeric habits
        string? Notes,
        int? MoodLevel, // 1-5
        int? EnergyLevel // 1-5
    );
}
