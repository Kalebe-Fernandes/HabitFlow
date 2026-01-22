namespace HabitFlow.Contracts.Users
{
    /// <summary>
    /// Request DTO for LGPD data access.
    /// </summary>
    public record DataAccessRequest(
        string[] DataCategories // Users, Habits, Goals, Gamification, Analytics
    );
}
