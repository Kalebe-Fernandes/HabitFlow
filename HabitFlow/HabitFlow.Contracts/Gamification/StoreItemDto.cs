namespace HabitFlow.Contracts.Gamification
{
    // Store DTOs
    public record StoreItemDto(
        int Id,
        string Name,
        string Description,
        string Category, // Theme, Icon, Avatar
        int Price,
        string ImageUrl,
        bool IsActive,
        bool IsPurchased,
        bool IsEquipped
    );
}
