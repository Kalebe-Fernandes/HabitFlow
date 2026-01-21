namespace HabitFlow.Domain.Common.Models
{
    /// <summary>
    /// Base class for all entities in the domain.
    /// Entities have a unique identifier and can change over time.
    /// </summary>
    /// <typeparam name="TId">The type of the entity identifier</typeparam>
    public abstract class Entity<TId> : IEquatable<Entity<TId>> where TId : notnull
    {
        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public TId Id { get; protected set; } = default!;

        /// <summary>
        /// Gets or sets the date and time when the entity was created.
        /// </summary>
        public DateTime CreatedAt { get; protected set; }

        /// <summary>
        /// Gets or sets the date and time when the entity was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; protected set; }

        protected Entity(TId id)
        {
            Id = id;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        protected Entity()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public override bool Equals(object? obj)
        {
            return obj is Entity<TId> entity && Equals(entity);
        }

        public bool Equals(Entity<TId>? other)
        {
            return other is not null && EqualityComparer<TId>.Default.Equals(Id, other.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
        {
            return !Equals(left, right);
        }
    }
}
