using HabitFlow.Domain.Common.Interfaces;

namespace HabitFlow.Domain.Gamification.Events
{
    public sealed record BadgeEarnedEvent(Guid UserId, int BadgeId, string BadgeName) : IDomainEvent
    {
        public Guid EventId { get; init; } = Guid.NewGuid();
        public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
        public string Version { get; init; } = "1.0";
    }
}
