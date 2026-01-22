namespace HabitFlow.Contracts.Goals
{
    /// <summary>
    /// Deep analysis DTO for a goal (RF-043).
    /// </summary>
    public record GoalAnalysisDto(
        Guid GoalId,
        string Name,
        GoalProgressDetailDto Progress,
        GoalTrendDto Trend,
        string[] Insights,
        string[] Alerts,
        string[] Recommendations
    );
}
