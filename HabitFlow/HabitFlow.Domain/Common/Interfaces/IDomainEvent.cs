namespace HabitFlow.Domain.Common.Interfaces
{
    /// <summary>
    /// Marker interface for domain events.
    /// Domain events represent something that happened in the domain that domain experts care about.
    /// </summary>
    public interface IDomainEvent
    {
        /// <summary>
        /// Gets the unique identifier for this domain event.
        /// </summary>
        Guid EventId { get; }

        /// <summary>
        /// Gets the date and time when the event occurred (UTC).
        /// </summary>
        DateTime OccurredOn { get; }

        /// <summary>
        /// Gets the version of the event schema.
        /// Format: Major.Minor (e.g., "1.0", "2.1")
        /// </summary>
        string Version { get; }
    }
}
