namespace HabitFlow.Contracts.Journaling
{
    /// <summary>
    /// Response DTO for mood and energy trends (RF-021).
    /// </summary>
    public record MoodEnergyTrendDto(
        DateTime Date,
        decimal? AverageMood,
        decimal? AverageEnergy,
        int CompletedHabits,
        decimal CompletionRate
    );
}
