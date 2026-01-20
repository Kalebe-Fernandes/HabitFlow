namespace HabitFlow.Domain.Common.Interfaces
{
    /// <summary>
    /// Interface for aggregates that can raise domain events.
    /// </summary>
    public interface IHasDomainEvents
    {
        /// <summary>
        /// Gets the collection of domain events raised by this aggregate.
        /// </summary>
        IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

        /// <summary>
        /// Clears all domain events from the aggregate.
        /// Should be called after events have been dispatched.
        /// </summary>
        void ClearDomainEvents();
    }
}
