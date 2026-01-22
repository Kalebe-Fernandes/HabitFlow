namespace HabitFlow.Contracts.Gamification
{
    /// <summary>
    /// Badge criteria request for badge creation.
    /// </summary>
    public record CreateBadgeCriteriaRequest(
        string Type, // Streak, TotalCompletions, ConsecutiveDays, Level, PerfectWeek, GoalCompletion
        int TargetValue,
        Guid? SpecificHabitId = null,
        int? SpecificCategoryId = null
    );
}
