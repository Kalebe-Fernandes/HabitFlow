namespace HabitFlow.Domain.Gamification.Enums
{
    /// <summary>
    /// Defines the types of virtual currency transactions.
    /// </summary>
    public enum TransactionType
    {
        /// <summary>
        /// Currency was earned by the user.
        /// </summary>
        Earned = 1,

        /// <summary>
        /// Currency was spent by the user.
        /// </summary>
        Spent = 2
    }
}
