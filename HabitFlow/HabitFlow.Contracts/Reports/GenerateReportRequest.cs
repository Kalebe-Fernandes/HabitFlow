namespace HabitFlow.Contracts.Reports
{
    /// <summary>
    /// Request DTO for generating a custom report.
    /// </summary>
    public record GenerateReportRequest(
        DateTime StartDate,
        DateTime EndDate,
        bool IncludeHabits = true,
        bool IncludeGoals = true,
        bool IncludeGamification = true,
        bool IncludeInsights = true,
        string Format = "PDF" // PDF, HTML, JSON
    );
}
