namespace HabitFlow.Contracts.Analytics
{
    /// <summary>
    /// Request DTO for monthly report generation.
    /// </summary>
    public record MonthlyReportRequest(
        int Year,
        int Month,
        string Format = "PDF" // PDF, CSV, JSON
    );
}
