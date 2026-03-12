using HabitFlow.Aplicacao.Features.Gamification.Commands.AwardXP;
using HabitFlow.Domain.Habits.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HabitFlow.Infrastructure.EventHandlers
{
    /// <summary>
    /// Awards XP to the user when a habit completion event is raised.
    /// The XP amount is taken from the event (HabitCompletedEvent.XpAwarded),
    /// which reflects the per-completion value configured in the habit aggregate.
    /// </summary>
    public sealed class HabitCompletedEventHandler(IMediator mediator, ILogger<HabitCompletedEventHandler> logger) : INotificationHandler<HabitCompletedEvent>
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<HabitCompletedEventHandler> _logger = logger;

        public async Task Handle(HabitCompletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Processing HabitCompletedEvent: HabitId={HabitId}, UserId={UserId}, XP={XP}",
                notification.HabitId,
                notification.UserId,
                notification.XpAwarded);

            var command = new AwardXPCommand(notification.UserId, notification.XpAwarded, $"Habit completed: {notification.HabitId}", notification.HabitId);
            var result = await _mediator.Send(command, cancellationToken);
            if (result.IsFailure)
            {
                _logger.LogWarning(
                    "Failed to award XP for habit completion: UserId={UserId}, Error={Error}",
                    notification.UserId,
                    result.Error);
            }
            else
            {
                _logger.LogInformation(
                    "XP awarded: UserId={UserId}, Amount={Amount}, TotalXP={TotalXP}, LeveledUp={LeveledUp}",
                    notification.UserId,
                    notification.XpAwarded,
                    result.Value.TotalXP,
                    result.Value.LeveledUp);
            }
        }
    }
}
