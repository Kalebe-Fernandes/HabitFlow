using HabitFlow.Domain.Common.Interfaces;

namespace HabitFlow.Domain.Goals.Events
{
    public sealed record GoalProgressUpdatedEvent(Guid GoalId, Guid UserId, decimal CurrentValue, decimal TargetValue) : IDomainEvent
    {
        public Guid EventId { get; init; } = Guid.NewGuid();
        public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
        public string Version { get; init; } = "1.0";
    }
}
