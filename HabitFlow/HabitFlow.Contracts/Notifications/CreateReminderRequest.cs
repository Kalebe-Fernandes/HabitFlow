namespace HabitFlow.Contracts.Notifications
{
    /// <summary>
    /// Request DTO for creating a habit reminder notification.
    /// </summary>
    public record CreateReminderRequest(
        Guid HabitId,
        DateTime ScheduledFor,
        string? CustomMessage = null
    );
}
