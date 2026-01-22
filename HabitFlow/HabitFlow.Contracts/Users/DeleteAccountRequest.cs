namespace HabitFlow.Contracts.Users
{
    /// <summary>
    /// Request DTO for deleting user account (LGPD compliance).
    /// </summary>
    public record DeleteAccountRequest(
        string Password, // Confirmation required
        string Reason, // Required for audit
        bool ConfirmDataLoss // Must be true
    );
}
