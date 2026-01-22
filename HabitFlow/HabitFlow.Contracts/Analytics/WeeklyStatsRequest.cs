namespace HabitFlow.Contracts.Analytics
{
    /// <summary>
    /// Request DTO for weekly statistics.
    /// </summary>
    public record WeeklyStatsRequest(
        DateTime? StartDate = null,
        DateTime? EndDate = null
    );
}
