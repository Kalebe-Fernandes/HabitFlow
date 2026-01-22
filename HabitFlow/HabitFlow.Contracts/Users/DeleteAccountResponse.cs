namespace HabitFlow.Contracts.Users
{
    /// <summary>
    /// Response DTO for account deletion confirmation.
    /// </summary>
    public record DeleteAccountResponse(
        Guid UserId,
        DateTime ScheduledDeletionDate, // 30 days from request
        string Status, // "Scheduled", "Cancelled", "Completed"
        string Message
    );
}
