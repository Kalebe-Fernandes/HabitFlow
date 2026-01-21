using HabitFlow.Domain.Common.Models;

namespace HabitFlow.Domain.Common.Interfaces
{
    /// <summary>
    /// Generic repository interface for aggregate roots.
    /// Defines the contract for data access operations.
    /// </summary>
    /// <typeparam name="TAggregate">The aggregate root type</typeparam>
    /// <typeparam name="TId">The type of the aggregate identifier</typeparam>
    public interface IRepository<TAggregate, TId>
        where TAggregate : AggregateRoot<TId>
        where TId : notnull
    {
        /// <summary>
        /// Gets an aggregate by its identifier.
        /// </summary>
        /// <param name="id">The aggregate identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The aggregate if found, null otherwise</returns>
        Task<TAggregate?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a new aggregate to the repository.
        /// </summary>
        /// <param name="aggregate">The aggregate to add</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task AddAsync(TAggregate aggregate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing aggregate in the repository.
        /// </summary>
        /// <param name="aggregate">The aggregate to update</param>
        void Update(TAggregate aggregate);

        /// <summary>
        /// Removes an aggregate from the repository.
        /// </summary>
        /// <param name="aggregate">The aggregate to remove</param>
        void Remove(TAggregate aggregate);
    }
}
