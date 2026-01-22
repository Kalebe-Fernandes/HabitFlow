namespace HabitFlow.Contracts.Analytics
{
    /// <summary>
    /// Response DTO for correlation analysis (RF-021).
    /// </summary>
    public record CorrelationDto(
        string Variable1, // e.g., "Mood", "Energy", "HabitCompletion"
        string Variable2,
        decimal CorrelationCoefficient, // Pearson correlation: -1 to 1
        string Interpretation, // "Strong positive", "Weak negative", etc.
        int DataPoints,
        string? Insight // Human-readable insight
    );
}
