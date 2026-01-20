using HabitFlow.Domain.Common.Models;

namespace HabitFlow.Domain.Users
{
    public sealed class UserSettings : ValueObject
    {
        public bool NotificationsEnabled { get; private set; } = true;
        public bool DarkMode { get; private set; } = false;
        public string Theme { get; private set; } = "default";
        private UserSettings() { }
        public static UserSettings CreateDefault() => new();
        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return NotificationsEnabled;
            yield return DarkMode;
            yield return Theme;
        }
    }
}
