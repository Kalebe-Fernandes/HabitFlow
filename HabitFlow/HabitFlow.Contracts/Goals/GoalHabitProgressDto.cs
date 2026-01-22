namespace HabitFlow.Contracts.Goals
{
    /// <summary>
    /// Individual habit contribution to goal progress.
    /// </summary>
    public record GoalHabitProgressDto(
        Guid HabitId,
        string HabitName,
        decimal ContributionWeight,
        decimal HabitValue,
        decimal WeightedContribution,
        int CompletionCount
    );
}
