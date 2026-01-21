using HabitFlow.Domain.Common.Interfaces;
using MediatR;

namespace HabitFlow.Domain.Gamification.Events
{
    public sealed record LevelUpEvent(Guid UserId, int NewLevel) : IDomainEvent, INotification
    {
        public Guid EventId { get; init; } = Guid.NewGuid();
        public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
        public string Version { get; init; } = "1.0";
    }
}
