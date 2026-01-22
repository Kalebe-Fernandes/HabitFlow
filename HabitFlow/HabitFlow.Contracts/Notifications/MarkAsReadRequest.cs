namespace HabitFlow.Contracts.Notifications
{
    /// <summary>
    /// Request DTO for marking notifications as read.
    /// </summary>
    public record MarkAsReadRequest(
        long[] NotificationIds
    );
}
