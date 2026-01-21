namespace HabitFlow.Contracts.Goals
{
    // Request DTOs
    public record CreateGoalRequest(
        string Name,
        string? Description,
        decimal TargetValue,
        string TargetUnit,
        DateTime StartDate,
        DateTime TargetDate
    );
}
