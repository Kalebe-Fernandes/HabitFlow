using HabitFlow.Domain.Common.Interfaces;

namespace HabitFlow.Domain.Habits.Events
{
    /// <summary>
    /// Domain event raised when a new habit is created.
    /// </summary>
    public sealed record HabitCreatedEvent(Guid HabitId, Guid UserId, string HabitName) : IDomainEvent
    {
        public Guid EventId { get; init; } = Guid.NewGuid();
        public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
        public string Version { get; init; } = "1.0";
    }
}
