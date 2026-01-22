namespace HabitFlow.Contracts.Gamification
{
    /// <summary>
    /// Response DTO for user inventory items.
    /// </summary>
    public record UserInventoryDto(
        long Id,
        Guid UserId,
        int StoreItemId,
        string ItemName,
        string ItemCategory,
        string ItemImageUrl,
        DateTime PurchasedAt,
        bool IsEquipped,
        DateTime? EquippedAt
    );
}
