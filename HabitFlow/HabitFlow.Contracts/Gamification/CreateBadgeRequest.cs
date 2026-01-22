namespace HabitFlow.Contracts.Gamification
{
    /// <summary>
    /// Request DTO for creating a new badge.
    /// </summary>
    public record CreateBadgeRequest(
        string Name,
        string Description,
        string IconUrl,
        string Rarity, // Common, Uncommon, Rare, Epic, Legendary
        CreateBadgeCriteriaRequest Criteria
    );
}
