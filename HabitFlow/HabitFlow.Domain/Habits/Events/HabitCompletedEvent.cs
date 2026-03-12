using HabitFlow.Domain.Common.Interfaces;
using MediatR;

namespace HabitFlow.Domain.Habits.Events
{
    public sealed record HabitCompletedEvent(Guid HabitId, Guid UserId, DateOnly CompletionDate, decimal? CompletedValue, int XPAwarded) : IDomainEvent, INotification
    {
        public Guid EventId { get; init; } = Guid.NewGuid();
        public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
        public string Version { get; init; } = "1.0";
        public int XpAwarded { get; set; }
    }
}
