namespace HabitFlow.Contracts.Users
{
    public record LoginResponse(
        string AccessToken,
        string RefreshToken,
        DateTime ExpiresAt,
        UserDto User
    );
}
