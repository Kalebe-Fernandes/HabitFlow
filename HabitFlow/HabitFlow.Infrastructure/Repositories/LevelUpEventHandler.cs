using HabitFlow.Domain.Gamification.Events;
using HabitFlow.Infrastructure.Services.Notifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HabitFlow.Infrastructure.Repositories
{
    /// <summary>
    /// Handles LevelUpEvent by sending a push notification to the user.
    /// </summary>
    public sealed class LevelUpEventHandler(INotificationService notificationService, ILogger<LevelUpEventHandler> logger) : INotificationHandler<LevelUpEvent>
    {
        private readonly INotificationService _notificationService = notificationService;
        private readonly ILogger<LevelUpEventHandler> _logger = logger;

        public async Task Handle(LevelUpEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Processing LevelUpEvent: UserId={UserId}, NewLevel={NewLevel}",
                notification.UserId,
                notification.NewLevel);

            await _notificationService.SendAchievementNotificationAsync(
                notification.UserId,
                $"Level {notification.NewLevel} reached",
                cancellationToken);
        }
    }
}
