namespace HabitFlow.Contracts.Users
{
    public record UserProfileDto(
        string? Bio,
        DateTime? DateOfBirth,
        string? Timezone,
        string? Language
    );
}
