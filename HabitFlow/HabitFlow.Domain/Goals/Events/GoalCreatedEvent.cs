using HabitFlow.Domain.Common.Interfaces;

namespace HabitFlow.Domain.Goals.Events
{
    public sealed record GoalCreatedEvent(Guid GoalId, Guid UserId, string GoalName) : IDomainEvent
    {
        public Guid EventId { get; init; } = Guid.NewGuid();
        public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
        public string Version { get; init; } = "1.0";
    }
}
