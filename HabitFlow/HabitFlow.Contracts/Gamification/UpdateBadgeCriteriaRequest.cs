namespace HabitFlow.Contracts.Gamification
{
    /// <summary>
    /// Badge criteria request for badge updates.
    /// </summary>
    public record UpdateBadgeCriteriaRequest(
        string Type, // Streak, TotalCompletions, ConsecutiveDays, Level, PerfectWeek, GoalCompletion
        int TargetValue,
        Guid? SpecificHabitId = null,
        int? SpecificCategoryId = null
    );
}
