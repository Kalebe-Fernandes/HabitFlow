namespace HabitFlow.Contracts.Gamification
{
    /// <summary>
    /// Request DTO for updating an existing badge.
    /// </summary>
    public record UpdateBadgeRequest(
        string Name,
        string Description,
        string IconUrl,
        string Rarity, // Common, Uncommon, Rare, Epic, Legendary
        UpdateBadgeCriteriaRequest? Criteria = null
    );
}
