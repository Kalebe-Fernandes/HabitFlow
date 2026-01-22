namespace HabitFlow.Contracts.Reports
{
    /// <summary>
    /// Insight generated from user data.
    /// </summary>
    public record ReportInsightDto(
        string Category, // Performance, Patterns, Recommendations
        string Title,
        string Description,
        string? ActionableAdvice,
        decimal? Confidence, // 0-1, how confident the system is in this insight
        string[] RelatedHabitNames
    );
}
