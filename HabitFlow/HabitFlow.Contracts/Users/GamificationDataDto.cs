namespace HabitFlow.Contracts.Users
{
    /// <summary>
    /// Gamification data for portability export.
    /// </summary>
    public record GamificationDataDto(
        int CurrentLevel,
        long TotalXP,
        int CurrentBalance,
        int TotalCoinsEarned,
        int BadgesEarned,
        string[] BadgeNames
    );
}
