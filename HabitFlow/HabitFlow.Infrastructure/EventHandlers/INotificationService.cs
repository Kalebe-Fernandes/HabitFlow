namespace HabitFlow.Infrastructure.EventHandlers
{
    public interface INotificationService
    {
        Task SendPushNotificationAsync(
            Guid userId,
            string title,
            string message,
            CancellationToken cancellationToken = default);
    }
}
