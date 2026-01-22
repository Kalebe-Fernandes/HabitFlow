namespace HabitFlow.Domain.Notifications.Enums
{
    /// <summary>
    /// Defines the types of notifications that can be sent to users.
    /// </summary>
    public enum NotificationType
    {
        /// <summary>
        /// Reminder notification for a habit.
        /// </summary>
        HabitReminder = 1,

        /// <summary>
        /// Achievement notification (badge earned, level up, etc.).
        /// </summary>
        Achievement = 2,

        /// <summary>
        /// Social notification (group invite, challenge, etc.).
        /// </summary>
        Social = 3,

        /// <summary>
        /// System notification (maintenance, updates, etc.).
        /// </summary>
        System = 4,

        /// <summary>
        /// Goal-related notification (progress, completion, etc.).
        /// </summary>
        Goal = 5,

        /// <summary>
        /// Streak warning notification (streak at risk).
        /// </summary>
        StreakWarning = 6
    }
}
