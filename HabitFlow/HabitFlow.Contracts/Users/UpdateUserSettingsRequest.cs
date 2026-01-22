namespace HabitFlow.Contracts.Users
{
    public record UpdateUserSettingsRequest(
        string? Timezone,
        string? Language,
        bool NotificationsEnabled = true
    );
}
