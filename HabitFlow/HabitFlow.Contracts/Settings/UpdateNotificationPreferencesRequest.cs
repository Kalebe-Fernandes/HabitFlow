namespace HabitFlow.Contracts.Settings
{
    /// <summary>
    /// Request DTO for updating notification preferences.
    /// </summary>
    public record UpdateNotificationPreferencesRequest(
        bool? HabitRemindersEnabled = null,
        bool? AchievementNotificationsEnabled = null,
        bool? SocialNotificationsEnabled = null,
        bool? EmailDigestEnabled = null,
        string? EmailDigestFrequency = null,
        TimeSpan? QuietHoursStart = null,
        TimeSpan? QuietHoursEnd = null
    );
}
