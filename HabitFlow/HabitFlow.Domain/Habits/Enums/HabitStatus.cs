namespace HabitFlow.Domain.Habits.Enums
{
    /// <summary>
    /// Represents the lifecycle status of a habit.
    /// </summary>
    public enum HabitStatus
    {
        /// <summary>
        /// Habit is active and being tracked.
        /// </summary>
        Active = 0,

        /// <summary>
        /// Habit is temporarily paused (e.g., vacation mode).
        /// </summary>
        Paused = 1,

        /// <summary>
        /// Habit is archived and no longer actively tracked.
        /// </summary>
        Archived = 2
    }
}
