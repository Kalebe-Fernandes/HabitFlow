namespace HabitFlow.Contracts.Common
{
    /// <summary>
    /// Individual component health status.
    /// </summary>
    public record ComponentHealthDto(
        string Status, // Healthy, Degraded, Unhealthy
        string? Description = null,
        TimeSpan? ResponseTime = null
    );
}
