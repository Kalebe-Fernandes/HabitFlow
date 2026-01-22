using HabitFlow.Domain.Common.Exceptions;
using HabitFlow.Domain.Common.Models;
using HabitFlow.Domain.Gamification.Enums;

namespace HabitFlow.Domain.Gamification
{
    /// <summary>
    /// Aggregate root representing an item available for purchase in the virtual store.
    /// Users can spend virtual currency to acquire these items for customization.
    /// </summary>
    public sealed class StoreItem : AggregateRoot<int>
    {
        private const int MaxNameLength = 100;
        private const int MaxDescriptionLength = 500;
        private const int MaxImageUrlLength = 500;

        /// <summary>
        /// Gets the item name.
        /// </summary>
        public string Name { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the item description.
        /// </summary>
        public string Description { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the category of the item (Theme, Icon, Avatar, etc.).
        /// </summary>
        public ItemCategory Category { get; private set; }

        /// <summary>
        /// Gets the price in virtual currency.
        /// </summary>
        public int Price { get; private set; }

        /// <summary>
        /// Gets the URL for the item's preview image.
        /// </summary>
        public string ImageUrl { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the data/configuration associated with this item (JSON format).
        /// For themes: color scheme JSON. For icons: icon identifier. etc.
        /// </summary>
        public string ItemData { get; private set; } = string.Empty;

        /// <summary>
        /// Gets whether this item is currently available for purchase.
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Gets whether this item is featured in the store.
        /// </summary>
        public bool IsFeatured { get; private set; }

        /// <summary>
        /// Gets the sort order for display purposes.
        /// </summary>
        public int SortOrder { get; private set; }

        private StoreItem() { }

        /// <summary>
        /// Creates a new store item.
        /// </summary>
        /// <param name="name">The item name.</param>
        /// <param name="description">The item description.</param>
        /// <param name="category">The item category.</param>
        /// <param name="price">The price in virtual currency.</param>
        /// <param name="imageUrl">The preview image URL.</param>
        /// <param name="itemData">The item configuration data.</param>
        /// <returns>A new StoreItem instance.</returns>
        /// <exception cref="ValidationException">Thrown when validation fails.</exception>
        public static StoreItem Create(string name, string description, ItemCategory category, int price, string imageUrl, string itemData)
        {
            ValidateName(name);
            ValidateDescription(description);
            ValidatePrice(price);
            ValidateImageUrl(imageUrl);

            return new StoreItem
            {
                Name = name.Trim(),
                Description = description.Trim(),
                Category = category,
                Price = price,
                ImageUrl = imageUrl.Trim(),
                ItemData = itemData?.Trim() ?? string.Empty,
                IsActive = true,
                IsFeatured = false,
                SortOrder = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Updates the item information.
        /// </summary>
        /// <param name="name">The new name.</param>
        /// <param name="description">The new description.</param>
        /// <param name="price">The new price.</param>
        /// <param name="imageUrl">The new image URL.</param>
        /// <param name="itemData">The new item data.</param>
        /// <exception cref="ValidationException">Thrown when validation fails.</exception>
        public void Update(string name, string description, int price, string imageUrl, string itemData)
        {
            ValidateName(name);
            ValidateDescription(description);
            ValidatePrice(price);
            ValidateImageUrl(imageUrl);

            Name = name.Trim();
            Description = description.Trim();
            Price = price;
            ImageUrl = imageUrl.Trim();
            ItemData = itemData?.Trim() ?? string.Empty;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Updates the item's category.
        /// </summary>
        /// <param name="category">The new category.</param>
        public void UpdateCategory(ItemCategory category)
        {
            Category = category;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Activates the item, making it available for purchase.
        /// </summary>
        public void Activate()
        {
            if (IsActive) return;
            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Deactivates the item, removing it from the store.
        /// </summary>
        public void Deactivate()
        {
            if (!IsActive) return;
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Marks the item as featured.
        /// </summary>
        public void Feature()
        {
            if (IsFeatured) return;
            IsFeatured = true;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Removes featured status from the item.
        /// </summary>
        public void Unfeature()
        {
            if (!IsFeatured) return;
            IsFeatured = false;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Updates the sort order for display.
        /// </summary>
        /// <param name="sortOrder">The new sort order.</param>
        public void UpdateSortOrder(int sortOrder)
        {
            SortOrder = sortOrder;
            UpdatedAt = DateTime.UtcNow;
        }

        private static void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException(nameof(name), "Item name cannot be empty");

            if (name.Length > MaxNameLength)
                throw new ValidationException(nameof(name), $"Item name cannot exceed {MaxNameLength} characters");
        }

        private static void ValidateDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ValidationException(nameof(description), "Item description cannot be empty");

            if (description.Length > MaxDescriptionLength)
                throw new ValidationException(nameof(description), $"Item description cannot exceed {MaxDescriptionLength} characters");
        }

        private static void ValidatePrice(int price)
        {
            if (price < 0)
                throw new ValidationException(nameof(price), "Price cannot be negative");
        }

        private static void ValidateImageUrl(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                throw new ValidationException(nameof(imageUrl), "Image URL cannot be empty");

            if (imageUrl.Length > MaxImageUrlLength)
                throw new ValidationException(nameof(imageUrl), $"Image URL cannot exceed {MaxImageUrlLength} characters");

            if (!Uri.TryCreate(imageUrl, UriKind.Absolute, out _))
                throw new ValidationException(nameof(imageUrl), "Image URL must be a valid URL");
        }
    }
}
