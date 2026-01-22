namespace HabitFlow.Contracts.Users
{
    /// <summary>
    /// Request DTO for cancelling account deletion.
    /// </summary>
    public record CancelDeletionRequest(
        string Password // Confirmation required
    );
}
