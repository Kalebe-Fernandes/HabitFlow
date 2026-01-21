namespace HabitFlow.Contracts.Goals
{
    public record AssociateHabitRequest(
        Guid HabitId,
        decimal ContributionWeight // 0-1, sum must equal 1
    );
}
