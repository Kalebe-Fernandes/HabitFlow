using HabitFlow.Domain.Common.Interfaces;

namespace HabitFlow.Domain.Common.Models
{
    /// <summary>
    /// Base class for aggregate roots in the domain.
    /// Aggregates are clusters of domain objects that can be treated as a single unit.
    /// </summary>
    /// <typeparam name="TId">The type of the aggregate identifier</typeparam>
    public abstract class AggregateRoot<TId> : Entity<TId>, IHasDomainEvents where TId : notnull
    {
        private readonly List<IDomainEvent> _domainEvents = new();

        /// <summary>
        /// Gets the collection of domain events raised by this aggregate.
        /// </summary>
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        /// <summary>
        /// Adds a domain event to the aggregate's event collection.
        /// </summary>
        /// <param name="domainEvent">The domain event to add</param>
        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        /// <summary>
        /// Clears all domain events from the aggregate.
        /// Should be called after events have been dispatched.
        /// </summary>
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        protected AggregateRoot(TId id) : base(id)
        {
        }

        protected AggregateRoot()
        {
        }
    }
}
