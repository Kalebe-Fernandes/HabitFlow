namespace HabitFlow.Contracts.Notifications
{
    /// <summary>
    /// Request DTO for listing notifications with filters.
    /// </summary>
    public record ListNotificationsRequest(
        string? Type = null, // Filter by type
        bool? IsRead = null, // Filter by read status
        int Page = 1,
        int PageSize = 20
    );
}
