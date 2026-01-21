namespace HabitFlow.Contracts.Users
{
    // Authentication DTOs
    public record RegisterRequest(
        string Email,
        string Password,
        string FirstName,
        string LastName,
        string? DisplayName = null
    );
}
