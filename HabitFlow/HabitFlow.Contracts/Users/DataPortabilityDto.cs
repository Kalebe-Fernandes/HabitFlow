namespace HabitFlow.Contracts.Users
{
    /// <summary>
    /// Response DTO for data portability request (LGPD compliance).
    /// </summary>
    public record DataPortabilityDto(
        Guid UserId,
        UserDataDto User,
        HabitDataDto[] Habits,
        GoalDataDto[] Goals,
        CompletionDataDto[] Completions,
        GamificationDataDto Gamification,
        DateTime ExportedAt,
        string Version
    );
}
