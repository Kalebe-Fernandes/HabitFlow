namespace HabitFlow.Contracts.Gamification
{
    /// <summary>
    /// Badge criteria DTO embedded in badge responses.
    /// </summary>
    public record BadgeCriteriaDto(
        string Type, // Streak, TotalCompletions, ConsecutiveDays, Level, PerfectWeek, GoalCompletion
        int TargetValue,
        Guid? SpecificHabitId,
        int? SpecificCategoryId,
        string Description
    );
}
