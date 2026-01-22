using HabitFlow.Domain.Common.Exceptions;
using HabitFlow.Domain.Common.Models;
using HabitFlow.Domain.Gamification.Enums;
using HabitFlow.Domain.Gamification.ValueObjects;

namespace HabitFlow.Domain.Gamification
{
    /// <summary>
    /// Aggregate root representing a badge/achievement in the gamification system.
    /// Defines the criteria for earning badges and their visual representation.
    /// </summary>
    public sealed class Badge : AggregateRoot<int>
    {
        private const int MaxNameLength = 100;
        private const int MaxDescriptionLength = 500;
        private const int MaxIconUrlLength = 500;

        /// <summary>
        /// Gets the badge name.
        /// </summary>
        public string Name { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the badge description explaining what it represents.
        /// </summary>
        public string Description { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the URL for the badge icon image.
        /// </summary>
        public string IconUrl { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the rarity level of the badge.
        /// </summary>
        public BadgeRarity Rarity { get; private set; }

        /// <summary>
        /// Gets the criteria that must be met to earn this badge.
        /// </summary>
        public BadgeCriteria Criteria { get; private set; } = null!;

        /// <summary>
        /// Gets whether this badge is currently active and can be earned.
        /// </summary>
        public bool IsActive { get; private set; }

        private Badge() { }

        /// <summary>
        /// Creates a new badge with the specified properties.
        /// </summary>
        /// <param name="name">The badge name.</param>
        /// <param name="description">The badge description.</param>
        /// <param name="iconUrl">The icon URL.</param>
        /// <param name="rarity">The rarity level.</param>
        /// <param name="criteria">The criteria for earning.</param>
        /// <returns>A new Badge instance.</returns>
        /// <exception cref="ValidationException">Thrown when validation fails.</exception>
        public static Badge Create(
            string name,
            string description,
            string iconUrl,
            BadgeRarity rarity,
            BadgeCriteria criteria)
        {
            ValidateName(name);
            ValidateDescription(description);
            ValidateIconUrl(iconUrl);

            return new Badge
            {
                Name = name.Trim(),
                Description = description.Trim(),
                IconUrl = iconUrl.Trim(),
                Rarity = rarity,
                Criteria = criteria,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Updates the badge information.
        /// </summary>
        /// <param name="name">The new name.</param>
        /// <param name="description">The new description.</param>
        /// <param name="iconUrl">The new icon URL.</param>
        /// <param name="rarity">The new rarity.</param>
        /// <exception cref="ValidationException">Thrown when validation fails.</exception>
        public void Update(string name, string description, string iconUrl, BadgeRarity rarity)
        {
            ValidateName(name);
            ValidateDescription(description);
            ValidateIconUrl(iconUrl);

            Name = name.Trim();
            Description = description.Trim();
            IconUrl = iconUrl.Trim();
            Rarity = rarity;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Updates the criteria for earning this badge.
        /// </summary>
        /// <param name="criteria">The new criteria.</param>
        public void UpdateCriteria(BadgeCriteria criteria)
        {
            Criteria = criteria ?? throw new ArgumentNullException(nameof(criteria));
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Activates the badge, making it available to be earned.
        /// </summary>
        public void Activate()
        {
            if (IsActive) return;
            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Deactivates the badge, preventing it from being earned.
        /// </summary>
        public void Deactivate()
        {
            if (!IsActive) return;
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }

        private static void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException(nameof(name), "Badge name cannot be empty");

            if (name.Length > MaxNameLength)
                throw new ValidationException(nameof(name), $"Badge name cannot exceed {MaxNameLength} characters");
        }

        private static void ValidateDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ValidationException(nameof(description), "Badge description cannot be empty");

            if (description.Length > MaxDescriptionLength)
                throw new ValidationException(nameof(description), $"Badge description cannot exceed {MaxDescriptionLength} characters");
        }

        private static void ValidateIconUrl(string iconUrl)
        {
            if (string.IsNullOrWhiteSpace(iconUrl))
                throw new ValidationException(nameof(iconUrl), "Icon URL cannot be empty");

            if (iconUrl.Length > MaxIconUrlLength)
                throw new ValidationException(nameof(iconUrl), $"Icon URL cannot exceed {MaxIconUrlLength} characters");

            if (!Uri.TryCreate(iconUrl, UriKind.Absolute, out _))
                throw new ValidationException(nameof(iconUrl), "Icon URL must be a valid URL");
        }
    }
}
