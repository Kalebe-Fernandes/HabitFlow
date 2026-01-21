namespace HabitFlow.Domain.Habits.Enums
{
    /// <summary>
    /// Represents the target type for a habit.
    /// </summary>
    public enum TargetType
    {
        /// <summary>
        /// Binary target (yes/no, completed/not completed).
        /// </summary>
        Binary = 0,

        /// <summary>
        /// Numeric target with a specific value to achieve.
        /// </summary>
        Numeric = 1
    }
}
