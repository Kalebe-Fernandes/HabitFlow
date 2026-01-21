namespace HabitFlow.Domain.Habits.Enums
{
    /// <summary>
    /// Represents the frequency type of a habit.
    /// </summary>
    public enum FrequencyType
    {
        /// <summary>
        /// Habit should be performed every day.
        /// </summary>
        Daily = 0,

        /// <summary>
        /// Habit should be performed specific days of the week.
        /// </summary>
        Weekly = 1,

        /// <summary>
        /// Habit has custom frequency configuration (specific days).
        /// </summary>
        Custom = 2
    }
}
