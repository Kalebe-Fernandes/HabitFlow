namespace HabitFlow.Contracts.Common
{
    /// <summary>
    /// Health check response for monitoring.
    /// </summary>
    public record HealthCheckResponse(
        string Status, // Healthy, Degraded, Unhealthy
        Dictionary<string, ComponentHealthDto> Components,
        DateTime CheckedAt,
        string Version
    );
}
