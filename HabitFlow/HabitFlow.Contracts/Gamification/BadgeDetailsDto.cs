namespace HabitFlow.Contracts.Gamification
{
    /// <summary>
    /// Response DTO for badge details.
    /// </summary>
    public record BadgeDetailsDto(
        int Id,
        string Name,
        string Description,
        string IconUrl,
        string Rarity, // Common, Uncommon, Rare, Epic, Legendary
        BadgeCriteriaDto Criteria,
        bool IsActive,
        DateTime CreatedAt,
        DateTime UpdatedAt
    );
}
