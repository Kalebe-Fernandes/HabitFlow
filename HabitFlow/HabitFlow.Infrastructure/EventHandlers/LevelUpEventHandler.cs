using HabitFlow.Domain.Gamification.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HabitFlow.Infrastructure.EventHandlers
{
    /// <summary>
    /// Handles LevelUpEvent by sending push notification.
    /// </summary>
    public sealed class LevelUpEventHandler(INotificationService notificationService,ILogger<LevelUpEventHandler> logger) : INotificationHandler<LevelUpEvent>
    {
        private readonly INotificationService _notificationService = notificationService;
        private readonly ILogger<LevelUpEventHandler> _logger = logger;

        public async Task Handle(LevelUpEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Processing LevelUpEvent: UserId={UserId}, NewLevel={NewLevel}",
                notification.UserId,
                notification.NewLevel);

            await _notificationService.SendPushNotificationAsync(
                notification.UserId,
                "Parabéns! 🎉",
                $"Você alcançou o nível {notification.NewLevel}!",
                cancellationToken);

            await Task.CompletedTask;
        }
    }
}
