namespace HabitFlow.Contracts.Gamification
{
    /// <summary>
    /// Response DTO for purchase confirmation.
    /// </summary>
    public record PurchaseItemResponse(
        long InventoryId,
        int ItemId,
        string ItemName,
        int Price,
        int NewBalance,
        DateTime PurchasedAt,
        string Message
    );
}
