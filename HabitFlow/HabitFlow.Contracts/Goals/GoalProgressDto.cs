namespace HabitFlow.Contracts.Goals
{
    public record GoalProgressDto(
        Guid GoalId,
        string GoalName,
        decimal ProgressPercentage,
        decimal ExpectedProgressPercentage,
        DateTime? ProjectedCompletionDate,
        List<string> Insights
    );
}
