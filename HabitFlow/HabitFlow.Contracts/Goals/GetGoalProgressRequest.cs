namespace HabitFlow.Contracts.Goals
{
    /// <summary>
    /// Request DTO for goal progress query.
    /// </summary>
    public record GetGoalProgressRequest(
        Guid GoalId,
        bool IncludeHabitDetails = true
    );
}
