using HabitFlow.Domain.Common.Interfaces;

namespace HabitFlow.Domain.Users.Events
{
    public sealed record UserProfileUpdatedEvent(Guid UserId, string DisplayName) : IDomainEvent
    {
        public Guid EventId { get; init; } = Guid.NewGuid();
        public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
        public string Version { get; init; } = "1.0";
    }
}
