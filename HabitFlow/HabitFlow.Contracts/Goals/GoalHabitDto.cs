namespace HabitFlow.Contracts.Goals
{
    public record GoalHabitDto(
        Guid HabitId,
        string HabitName,
        string IconName,
        decimal ContributionWeight,
        decimal ContributionValue, // Calculated value
        decimal ProgressPercentage
    );
}
