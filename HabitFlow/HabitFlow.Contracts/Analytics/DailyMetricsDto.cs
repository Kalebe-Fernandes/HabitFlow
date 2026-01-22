namespace HabitFlow.Contracts.Analytics
{
    public record DailyMetricsDto(
    DateTime Date,
    int CompletedHabits,
    int TotalHabits,
    decimal CompletionRate,
    int XPEarned,
    int CoinsEarned,
    decimal? AverageMood,
    decimal? AverageEnergy);
}
