using HabitFlow.Domain.Common.Exceptions;
using HabitFlow.Domain.Common.Models;
using HabitFlow.Domain.Gamification.Enums;

namespace HabitFlow.Domain.Gamification
{
    /// <summary>
    /// Aggregate root representing a user's virtual currency balance and transaction history.
    /// Tracks all currency earnings and spendings for audit and reporting purposes.
    /// </summary>
    public sealed class VirtualCurrency : AggregateRoot<Guid>
    {
        private readonly List<CurrencyTransaction> _transactions = [];

        /// <summary>
        /// Gets the current balance of virtual currency.
        /// </summary>
        public int CurrentBalance { get; private set; }

        /// <summary>
        /// Gets the total amount of currency earned lifetime.
        /// </summary>
        public long TotalEarned { get; private set; }

        /// <summary>
        /// Gets the total amount of currency spent lifetime.
        /// </summary>
        public long TotalSpent { get; private set; }

        /// <summary>
        /// Gets the read-only collection of transactions.
        /// </summary>
        public IReadOnlyCollection<CurrencyTransaction> Transactions => _transactions.AsReadOnly();

        private VirtualCurrency() { }

        /// <summary>
        /// Creates a new virtual currency account for a user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A new VirtualCurrency instance with zero balance.</returns>
        public static VirtualCurrency Create(Guid userId)
        {
            return new VirtualCurrency
            {
                Id = userId,
                CurrentBalance = 0,
                TotalEarned = 0,
                TotalSpent = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Adds currency to the user's balance.
        /// </summary>
        /// <param name="amount">The amount to add (must be positive).</param>
        /// <param name="reason">The reason for the earning.</param>
        /// <param name="relatedEntityId">Optional related entity (habit, goal, etc.).</param>
        /// <exception cref="ValidationException">Thrown when amount is not positive.</exception>
        public void Earn(int amount, string reason, Guid? relatedEntityId = null)
        {
            if (amount <= 0)
                throw new ValidationException(nameof(amount), "Amount must be positive");

            if (string.IsNullOrWhiteSpace(reason))
                throw new ValidationException(nameof(reason), "Reason cannot be empty");

            CurrentBalance += amount;
            TotalEarned += amount;

            var transaction = CurrencyTransaction.CreateEarned(Id, amount, CurrentBalance, reason, relatedEntityId);
            _transactions.Add(transaction);
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Deducts currency from the user's balance.
        /// </summary>
        /// <param name="amount">The amount to deduct (must be positive).</param>
        /// <param name="reason">The reason for the spending.</param>
        /// <param name="relatedEntityId">Optional related entity (store item, etc.).</param>
        /// <exception cref="ValidationException">Thrown when amount is not positive.</exception>
        /// <exception cref="DomainException">Thrown when insufficient balance.</exception>
        public void Spend(int amount, string reason, Guid? relatedEntityId = null)
        {
            if (amount <= 0)
                throw new ValidationException(nameof(amount), "Amount must be positive");

            if (string.IsNullOrWhiteSpace(reason))
                throw new ValidationException(nameof(reason), "Reason cannot be empty");

            if (CurrentBalance < amount)
                throw new DomainException($"Insufficient balance. Current: {CurrentBalance}, Required: {amount}", "INSUFFICIENT_CURRENCY");

            CurrentBalance -= amount;
            TotalSpent += amount;

            var transaction = CurrencyTransaction.CreateSpent(Id, amount, CurrentBalance, reason, relatedEntityId);
            _transactions.Add(transaction);
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Checks if the user has sufficient balance for a purchase.
        /// </summary>
        /// <param name="amount">The amount to check.</param>
        /// <returns>True if balance is sufficient, false otherwise.</returns>
        public bool HasSufficientBalance(int amount) => CurrentBalance >= amount;

        /// <summary>
        /// Gets the transaction history for a specific date range.
        /// </summary>
        /// <param name="startDate">The start date (inclusive).</param>
        /// <param name="endDate">The end date (inclusive).</param>
        /// <returns>List of transactions within the date range.</returns>
        public IReadOnlyList<CurrencyTransaction> GetTransactionHistory(DateTime startDate, DateTime endDate)
        {
            return _transactions
                .Where(t => t.CreatedAt >= startDate && t.CreatedAt <= endDate)
                .OrderByDescending(t => t.CreatedAt)
                .ToList()
                .AsReadOnly();
        }

        /// <summary>
        /// Gets the total earnings for a specific date range.
        /// </summary>
        /// <param name="startDate">The start date (inclusive).</param>
        /// <param name="endDate">The end date (inclusive).</param>
        /// <returns>Total currency earned in the period.</returns>
        public long GetTotalEarningsInPeriod(DateTime startDate, DateTime endDate)
        {
            return _transactions
                .Where(t => t.Type == TransactionType.Earned &&
                           t.CreatedAt >= startDate &&
                           t.CreatedAt <= endDate)
                .Sum(t => t.Amount);
        }

        /// <summary>
        /// Gets the total spendings for a specific date range.
        /// </summary>
        /// <param name="startDate">The start date (inclusive).</param>
        /// <param name="endDate">The end date (inclusive).</param>
        /// <returns>Total currency spent in the period.</returns>
        public long GetTotalSpendingsInPeriod(DateTime startDate, DateTime endDate)
        {
            return _transactions
                .Where(t => t.Type == TransactionType.Spent &&
                           t.CreatedAt >= startDate &&
                           t.CreatedAt <= endDate)
                .Sum(t => t.Amount);
        }
    }
}
