using HabitFlow.Domain.Common.Interfaces;

namespace HabitFlow.Domain.Habits.Events
{
    /// <summary>
    /// Domain event raised when a streak milestone is achieved.
    /// </summary>
    public sealed record StreakAchievedEvent(Guid HabitId, Guid UserId, int StreakCount) : IDomainEvent
    {
        public Guid EventId { get; init; } = Guid.NewGuid();
        public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
        public string Version { get; init; } = "1.0";
    }
}
