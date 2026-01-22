namespace HabitFlow.Infrastructure.Services.Notifications
{
    public interface INotificationService
    {
        Task SendPushNotificationAsync(Guid userId, string title, string body, CancellationToken cancellationToken = default);
        Task SendAchievementNotificationAsync(Guid userId, string achievementName, CancellationToken cancellationToken = default);
    }
}
