namespace HabitFlow.Contracts.Users
{
    public record UserDto(
        Guid Id,
        string Email,
        string FirstName,
        string LastName,
        string DisplayName,
        string? AvatarUrl,
        bool IsEmailVerified,
        UserProfileDto Profile,
        DateTime CreatedAt
    );
}
