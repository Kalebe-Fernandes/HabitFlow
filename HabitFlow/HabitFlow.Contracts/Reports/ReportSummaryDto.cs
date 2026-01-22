namespace HabitFlow.Contracts.Reports
{
    /// <summary>
    /// Summary statistics for a report period.
    /// </summary>
    public record ReportSummaryDto(
        int TotalCompletions,
        decimal AverageCompletionRate,
        decimal CompletionRateChange, // Percentage change from previous period
        int XPEarned,
        int XPChange,
        int BadgesEarned,
        int NewStreaksStarted,
        int StreaksMaintained,
        int StreaksLost,
        decimal AverageMood,
        decimal AverageEnergy
    );
}
