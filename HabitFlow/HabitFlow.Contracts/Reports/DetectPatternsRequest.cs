namespace HabitFlow.Contracts.Reports
{
    /// <summary>
    /// Request DTO for pattern detection.
    /// </summary>
    public record DetectPatternsRequest(
        DateTime? StartDate = null,
        DateTime? EndDate = null,
        Guid[]? SpecificHabitIds = null,
        int MinimumDataPoints = 10,
        decimal MinimumConfidence = 0.7m
    );
}
