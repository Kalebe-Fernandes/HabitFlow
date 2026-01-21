using HabitFlow.Domain.Common.Interfaces;

namespace HabitFlow.Domain.Gamification.Events
{
    public sealed record XPAwardedEvent(Guid UserId, int Amount, long TotalXP, string Reason, Guid? RelatedEntityId) : IDomainEvent
    {
        public Guid EventId { get; init; } = Guid.NewGuid();
        public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
        public string Version { get; init; } = "1.0";
    }
}
