namespace HabitFlow.Aplicacao.Features.Goals.Queries.GetGoals
{
    public record GoalDto(
        Guid Id,
        string Name,
        string? Description,
        decimal TargetValue,
        string TargetUnit,
        decimal CurrentValue,
        decimal ProgressPercentage,
        DateTime StartDate,
        DateTime TargetDate,
        string Status
    );
}
