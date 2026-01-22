using HabitFlow.Domain.Common.Models;

namespace HabitFlow.Domain.Gamification
{
    /// <summary>
    /// Entity representing a single XP (Experience Points) transaction.
    /// Provides immutable audit trail of all XP awards.
    /// </summary>
    public sealed class XPTransaction : Entity<long>
    {
        /// <summary>
        /// Gets the user identifier who received this XP.
        /// </summary>
        public Guid UserId { get; private set; }

        /// <summary>
        /// Gets the amount of XP awarded in this transaction.
        /// </summary>
        public int Amount { get; private set; }

        /// <summary>
        /// Gets the total XP after this transaction was applied.
        /// </summary>
        public long TotalXPAfter { get; private set; }

        /// <summary>
        /// Gets the reason or description for this XP award.
        /// </summary>
        public string Reason { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the related entity ID if applicable (habit, goal, etc.).
        /// </summary>
        public Guid? RelatedEntityId { get; private set; }

        /// <summary>
        /// Gets the level the user had after this transaction.
        /// </summary>
        public int LevelAfter { get; private set; }

        private XPTransaction() { }

        /// <summary>
        /// Creates a new XP transaction record.
        /// </summary>
        /// <param name="userId">The user who received the XP.</param>
        /// <param name="amount">The amount of XP awarded.</param>
        /// <param name="totalXPAfter">The total XP after this award.</param>
        /// <param name="levelAfter">The level after this award.</param>
        /// <param name="reason">The reason for the award.</param>
        /// <param name="relatedEntityId">Optional related entity.</param>
        /// <returns>A new XPTransaction instance.</returns>
        public static XPTransaction Create(Guid userId, int amount, long totalXPAfter, int levelAfter, string reason, Guid? relatedEntityId = null)
        {
            return new XPTransaction
            {
                UserId = userId,
                Amount = amount,
                TotalXPAfter = totalXPAfter,
                LevelAfter = levelAfter,
                Reason = reason,
                RelatedEntityId = relatedEntityId,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
