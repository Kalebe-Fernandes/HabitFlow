using HabitFlow.Domain.Common.Interfaces;

namespace HabitFlow.Domain.Habits.Events
{
    /// <summary>
    /// Domain event raised when a streak is broken.
    /// </summary>
    public sealed record StreakBrokenEvent(Guid HabitId, Guid UserId, int PreviousStreak, DateOnly BrokenDate) : IDomainEvent
    {
        public Guid EventId { get; init; } = Guid.NewGuid();
        public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
        public string Version { get; init; } = "1.0";
    }
}
