namespace HabitFlow.Contracts.Reports
{
    /// <summary>
    /// Response DTO for automated weekly/monthly reports (RF-036).
    /// </summary>
    public record AutomatedReportDto(
        string ReportId,
        string Period, // Weekly, Monthly
        DateTime PeriodStart,
        DateTime PeriodEnd,
        ReportSummaryDto Summary,
        ReportHighlightDto[] Highlights,
        ReportInsightDto[] Insights,
        string[] PersonalizedTips,
        DateTime GeneratedAt
    );
}
