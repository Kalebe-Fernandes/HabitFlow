namespace HabitFlow.Contracts.Gamification
{
    // Badge DTOs
    public record BadgeDto(
        int Id,
        string Name,
        string Description,
        string IconUrl,
        string Rarity, // Common, Uncommon, Rare, Epic, Legendary
        string CriteriaType,
        string CriteriaDescription,
        bool IsEarned,
        DateTime? EarnedAt
    );
}
