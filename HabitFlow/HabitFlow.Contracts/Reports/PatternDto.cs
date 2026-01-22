namespace HabitFlow.Contracts.Reports
{
    /// <summary>
    /// Response DTO for pattern detection (RF-042).
    /// </summary>
    public record PatternDto(
        string PatternType, // DayOfWeek, TimeOfDay, Correlation, Sequence
        string Description,
        decimal Confidence, // 0-1
        string Insight,
        PatternDataPointDto[] DataPoints,
        string[] AffectedHabits
    );
}
