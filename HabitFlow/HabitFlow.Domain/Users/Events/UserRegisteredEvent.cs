using HabitFlow.Domain.Common.Interfaces;

namespace HabitFlow.Domain.Users.Events
{
    public sealed record UserRegisteredEvent(Guid UserId, string Email, string DisplayName) : IDomainEvent
    {
        public Guid EventId { get; init; } = Guid.NewGuid();
        public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
        public string Version { get; init; } = "1.0";
    }
}
