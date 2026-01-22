namespace HabitFlow.Contracts.Goals
{
    /// <summary>
    /// Request DTO for goal analysis query.
    /// </summary>
    public record GetGoalAnalysisRequest(
        Guid GoalId,
        bool IncludePredictions = true,
        bool IncludeRecommendations = true
    );
}
