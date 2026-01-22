namespace HabitFlow.Contracts.Habits
{
    /// <summary>
    /// Request DTO for getting dashboard data.
    /// </summary>
    public record GetDashboardRequest(
        DateTime? Date = null // Defaults to today
    );
}
