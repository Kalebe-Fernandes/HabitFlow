namespace HabitFlow.Contracts.Notifications
{
    /// <summary>
    /// Response DTO for notifications.
    /// </summary>
    public record NotificationDto(
        long Id,
        Guid UserId,
        string Type, // HabitReminder, Achievement, Social, System, Goal, StreakWarning
        string Title,
        string Body,
        string? Data, // JSON data for client-side handling
        bool IsRead,
        bool IsSent,
        DateTime? ScheduledFor,
        DateTime? SentAt,
        DateTime? ReadAt,
        DateTime CreatedAt
    );
}
