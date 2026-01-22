namespace HabitFlow.Contracts.Users
{
    public record RegisterResponse(
        Guid UserId,
        string Email,
        string DisplayName
    );
}
