namespace HabitFlow.Contracts.Goals
{
    public record UpdateGoalRequest(
        string Name,
        string? Description,
        DateTime TargetDate
    );
}
