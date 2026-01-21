using HabitFlow.Domain.Common.Interfaces;

namespace HabitFlow.Domain.Gamification.Events
{
    public sealed record CurrencyEarnedEvent(Guid UserId, int Amount, int NewBalance, string Reason) : IDomainEvent
    {
        public Guid EventId { get; init; } = Guid.NewGuid();
        public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
        public string Version { get; init; } = "1.0";
    }
}
