using HabitFlow.Domain.Common.Interfaces;
using HabitFlow.Domain.Gamification;
using HabitFlow.Domain.Gamification.Enums;

namespace HabitFlow.Domain.Repositories
{
    /// <summary>
    /// Repository interface for Badge aggregate.
    /// Defines operations for badge persistence and retrieval.
    /// </summary>
    public interface IBadgeRepository : IRepository<Badge, int>
    {
        /// <summary>
        /// Gets all active badges ordered by rarity and name.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>List of active badges.</returns>
        Task<IReadOnlyList<Badge>> GetActiveBadgesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets badges by rarity level.
        /// </summary>
        /// <param name="rarity">The rarity level to filter by.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>List of badges with the specified rarity.</returns>
        Task<IReadOnlyList<Badge>> GetByRarityAsync(BadgeRarity rarity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets badges by criteria type.
        /// </summary>
        /// <param name="criteriaType">The criteria type to filter by.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>List of badges with the specified criteria type.</returns>
        Task<IReadOnlyList<Badge>> GetByCriteriaTypeAsync(BadgeCriteriaType criteriaType, CancellationToken cancellationToken = default);
    }
}
