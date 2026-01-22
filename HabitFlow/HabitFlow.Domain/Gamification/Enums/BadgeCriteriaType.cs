namespace HabitFlow.Domain.Gamification.Enums
{
    /// <summary>
    /// Defines the types of criteria that can be used for badge requirements.
    /// </summary>
    public enum BadgeCriteriaType
    {
        /// <summary>
        /// Badge earned by maintaining a streak of consecutive completions.
        /// </summary>
        Streak = 1,

        /// <summary>
        /// Badge earned by reaching a total number of completions.
        /// </summary>
        TotalCompletions = 2,

        /// <summary>
        /// Badge earned by completing habits for consecutive days.
        /// </summary>
        ConsecutiveDays = 3,

        /// <summary>
        /// Badge earned by reaching a specific level.
        /// </summary>
        Level = 4,

        /// <summary>
        /// Badge earned by completing all habits for an entire week.
        /// </summary>
        PerfectWeek = 5,

        /// <summary>
        /// Badge earned by completing a certain number of goals.
        /// </summary>
        GoalCompletion = 6
    }
}
