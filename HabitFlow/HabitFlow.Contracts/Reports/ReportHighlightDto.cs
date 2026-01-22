namespace HabitFlow.Contracts.Reports
{
    /// <summary>
    /// Highlight from the report period.
    /// </summary>
    public record ReportHighlightDto(
        string Type, // BestDay, LongestStreak, MostImproved, NewBadge
        string Title,
        string Description,
        DateTime? Date,
        string? IconName
    );
}
