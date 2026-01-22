namespace HabitFlow.Contracts.Settings
{
    /// <summary>
    /// Notification preferences DTO.
    /// </summary>
    public record NotificationPreferencesDto(
        bool HabitRemindersEnabled,
        bool AchievementNotificationsEnabled,
        bool SocialNotificationsEnabled,
        bool EmailDigestEnabled,
        string EmailDigestFrequency, // Daily, Weekly, Monthly
        TimeSpan? QuietHoursStart,
        TimeSpan? QuietHoursEnd
    );
}
