namespace HabitFlow.Contracts.Goals
{
    /// <summary>
    /// Trend analysis for goal progress.
    /// </summary>
    public record GoalTrendDto(
        string Direction, // Improving, Declining, Stable
        decimal WeeklyChangePercentage,
        decimal MonthlyChangePercentage,
        bool IsAccelerating,
        string? TrendDescription
    );
}
