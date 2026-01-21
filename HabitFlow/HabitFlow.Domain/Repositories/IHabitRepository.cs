using HabitFlow.Domain.Common.Interfaces;
using HabitFlow.Domain.Habits;

namespace HabitFlow.Domain.Repositories
{
    /// <summary>
    /// Repository interface for Habit aggregate.
    /// </summary>
    public interface IHabitRepository : IRepository<Habit, Guid>
    {
        /// <summary>
        /// Gets all active habits for a specific user.
        /// </summary>
        /// <param name="userId">The user identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of active habits</returns>
        Task<IEnumerable<Habit>> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets habits by user with pagination.
        /// </summary>
        /// <param name="userId">The user identifier</param>
        /// <param name="page">Page number (1-based)</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Paged collection of habits</returns>
        Task<(IEnumerable<Habit> Habits, int TotalCount)> GetPagedByUserIdAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a habit with its completions eagerly loaded.
        /// </summary>
        /// <param name="habitId">The habit identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The habit with completions if found, null otherwise</returns>
        Task<Habit?> GetByIdWithCompletionsAsync(Guid habitId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a habit name already exists for a user.
        /// </summary>
        /// <param name="userId">The user identifier</param>
        /// <param name="name">The habit name</param>
        /// <param name="excludeHabitId">Optional habit ID to exclude from the check (for updates)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if name exists, false otherwise</returns>
        Task<bool> ExistsWithNameAsync(Guid userId, string name, Guid? excludeHabitId = null, CancellationToken cancellationToken = default);
    }
}
