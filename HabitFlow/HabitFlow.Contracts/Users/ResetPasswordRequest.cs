namespace HabitFlow.Contracts.Users
{
    public record ResetPasswordRequest(
        string Token,
        string NewPassword
    );
}
