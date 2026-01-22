using HabitFlow.Domain.Common.Models;
using HabitFlow.Domain.Gamification.Enums;

namespace HabitFlow.Domain.Gamification
{
    /// <summary>
    /// Entity representing a single virtual currency transaction.
    /// Provides immutable audit trail of all currency movements.
    /// </summary>
    public sealed class CurrencyTransaction : Entity<long>
    {
        /// <summary>
        /// Gets the user identifier who owns this transaction.
        /// </summary>
        public Guid UserId { get; private set; }

        /// <summary>
        /// Gets the type of transaction (Earned or Spent).
        /// </summary>
        public TransactionType Type { get; private set; }

        /// <summary>
        /// Gets the amount of currency involved in this transaction.
        /// </summary>
        public int Amount { get; private set; }

        /// <summary>
        /// Gets the balance after this transaction was applied.
        /// </summary>
        public int BalanceAfter { get; private set; }

        /// <summary>
        /// Gets the reason or description for this transaction.
        /// </summary>
        public string Reason { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the related entity ID if applicable (habit, store item, etc.).
        /// </summary>
        public Guid? RelatedEntityId { get; private set; }

        private CurrencyTransaction() { }

        /// <summary>
        /// Creates a transaction for currency earned.
        /// </summary>
        /// <param name="userId">The user who earned the currency.</param>
        /// <param name="amount">The amount earned.</param>
        /// <param name="balanceAfter">The new balance after earning.</param>
        /// <param name="reason">The reason for earning.</param>
        /// <param name="relatedEntityId">Optional related entity.</param>
        /// <returns>A new CurrencyTransaction for earnings.</returns>
        public static CurrencyTransaction CreateEarned(Guid userId, int amount, int balanceAfter, string reason, Guid? relatedEntityId = null)
        {
            return new CurrencyTransaction
            {
                UserId = userId,
                Type = TransactionType.Earned,
                Amount = amount,
                BalanceAfter = balanceAfter,
                Reason = reason,
                RelatedEntityId = relatedEntityId,
                CreatedAt = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Creates a transaction for currency spent.
        /// </summary>
        /// <param name="userId">The user who spent the currency.</param>
        /// <param name="amount">The amount spent.</param>
        /// <param name="balanceAfter">The new balance after spending.</param>
        /// <param name="reason">The reason for spending.</param>
        /// <param name="relatedEntityId">Optional related entity.</param>
        /// <returns>A new CurrencyTransaction for spendings.</returns>
        public static CurrencyTransaction CreateSpent(Guid userId, int amount, int balanceAfter, string reason, Guid? relatedEntityId = null)
        {
            return new CurrencyTransaction
            {
                UserId = userId,
                Type = TransactionType.Spent,
                Amount = amount,
                BalanceAfter = balanceAfter,
                Reason = reason,
                RelatedEntityId = relatedEntityId,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
