namespace HabitFlow.Contracts.Users
{
    // Profile DTOs
    public record UpdateProfileRequest(
        string FirstName,
        string LastName,
        string DisplayName,
        string? Bio = null,
        string? AvatarUrl = null
    );
}
