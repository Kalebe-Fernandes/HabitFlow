using HabitFlow.Domain.Common.Exceptions;
using HabitFlow.Domain.Common.Models;
using HabitFlow.Domain.Users.Events;

namespace HabitFlow.Domain.Users
{
    public sealed class User : AggregateRoot<Guid>
    {
        private const int MaxEmailLength = 255;
        private const int MaxNameLength = 100;
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string DisplayName { get; private set; } = string.Empty;
        public string? AvatarUrl { get; private set; }
        public UserProfile Profile { get; private set; } = null!;
        public UserSettings Settings { get; private set; } = null!;
        public bool IsEmailVerified { get; private set; }
        public bool IsActive { get; private set; }
        private User() { }
        private User(string email, string passwordHash, string firstName, string lastName, string displayName)
        {
            Id = Guid.NewGuid();
            Email = email;
            PasswordHash = passwordHash;
            FirstName = firstName;
            LastName = lastName;
            DisplayName = displayName;
            Profile = UserProfile.CreateDefault();
            Settings = UserSettings.CreateDefault();
            IsEmailVerified = false;
            IsActive = true;
            AddDomainEvent(new UserRegisteredEvent(Id, email, displayName));
        }
        public static User Create(string email, string passwordHash, string firstName, string lastName, string? displayName = null)
        {
            ValidateInputs(email, passwordHash, firstName, lastName);
            var finalDisplayName = string.IsNullOrWhiteSpace(displayName) ? $"{firstName} {lastName}".Trim() : displayName.Trim();
            return new User(email.Trim().ToLowerInvariant(), passwordHash, firstName.Trim(), lastName.Trim(), finalDisplayName);
        }
        public void UpdateProfile(string firstName, string lastName, string displayName)
        {
            if (string.IsNullOrWhiteSpace(firstName)) throw new ValidationException(nameof(firstName), "First name is required");
            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            DisplayName = displayName.Trim();
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new UserProfileUpdatedEvent(Id, DisplayName));
        }
        public void UpdatePassword(string newPasswordHash)
        {
            PasswordHash = newPasswordHash;
            UpdatedAt = DateTime.UtcNow;
        }
        public void VerifyEmail()
        {
            IsEmailVerified = true;
            UpdatedAt = DateTime.UtcNow;
        }
        private static void ValidateInputs(string email, string passwordHash, string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ValidationException(nameof(email), "Email is required");
            if (string.IsNullOrWhiteSpace(firstName)) throw new ValidationException(nameof(firstName), "First name is required");
            if (string.IsNullOrWhiteSpace(lastName)) throw new ValidationException(nameof(lastName), "Last name is required");
            if (!VerifyPassword(passwordHash)) throw new ValidationException(nameof(firstName), "Password is not satisfied");
        }

        public static bool VerifyPassword(string password)
        {
            return password != null && password.Length > 8 && password.Length < 100;
        }
    }
}
