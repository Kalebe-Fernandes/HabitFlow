using Microsoft.Extensions.Logging;

namespace HabitFlow.Infrastructure.Services.Notifications
{
    public class MockNotificationService(ILogger<MockNotificationService> logger) : INotificationService
    {
        private readonly ILogger<MockNotificationService> _logger = logger;

        public Task SendPushNotificationAsync(Guid userId, string title, string body, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("[MOCK NOTIFICATION] Push to User {UserId}: {Title} - {Body}", userId, title, body);
            return Task.CompletedTask;
        }

        public Task SendAchievementNotificationAsync(Guid userId, string achievementName, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("[MOCK NOTIFICATION] Achievement to User {UserId}: {Achievement}", userId, achievementName);
            return Task.CompletedTask;
        }
    }
}
