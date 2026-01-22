namespace HabitFlow.Contracts.Users
{
    /// <summary>
    /// User data for portability export.
    /// </summary>
    public record UserDataDto(
        Guid Id,
        string Email,
        string FirstName,
        string LastName,
        string? Bio,
        DateTime? DateOfBirth,
        string Timezone,
        string Language,
        DateTime CreatedAt
    );
}
