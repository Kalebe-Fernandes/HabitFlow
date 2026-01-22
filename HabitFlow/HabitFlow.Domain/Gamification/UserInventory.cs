using HabitFlow.Domain.Common.Models;

namespace HabitFlow.Domain.Gamification
{
    /// <summary>
    /// Entity representing an item owned by a user.
    /// Part of the StoreItem aggregate boundary, tracking user purchases.
    /// </summary>
    public sealed class UserInventory : Entity<long>
    {
        /// <summary>
        /// Gets the user identifier who owns this item.
        /// </summary>
        public Guid UserId { get; private set; }

        /// <summary>
        /// Gets the store item identifier that was purchased.
        /// </summary>
        public int StoreItemId { get; private set; }

        /// <summary>
        /// Gets the date and time when the item was purchased.
        /// </summary>
        public DateTime PurchasedAt { get; private set; }

        /// <summary>
        /// Gets whether this item is currently equipped/active.
        /// </summary>
        public bool IsEquipped { get; private set; }

        /// <summary>
        /// Gets the date and time when the item was equipped (null if never equipped).
        /// </summary>
        public DateTime? EquippedAt { get; private set; }

        /// <summary>
        /// Navigation property to the store item.
        /// </summary>
        public StoreItem? StoreItem { get; private set; }

        private UserInventory() { }

        /// <summary>
        /// Creates a new inventory entry when a user purchases an item.
        /// </summary>
        /// <param name="userId">The user who purchased the item.</param>
        /// <param name="storeItemId">The item that was purchased.</param>
        /// <returns>A new UserInventory instance.</returns>
        public static UserInventory Create(Guid userId, int storeItemId)
        {
            return new UserInventory
            {
                UserId = userId,
                StoreItemId = storeItemId,
                PurchasedAt = DateTime.UtcNow,
                IsEquipped = false,
                EquippedAt = null
            };
        }

        /// <summary>
        /// Equips this item, making it active for the user.
        /// </summary>
        public void Equip()
        {
            if (IsEquipped) return;
            IsEquipped = true;
            EquippedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Unequips this item, making it inactive.
        /// </summary>
        public void Unequip()
        {
            if (!IsEquipped) return;
            IsEquipped = false;
        }
    }
}
