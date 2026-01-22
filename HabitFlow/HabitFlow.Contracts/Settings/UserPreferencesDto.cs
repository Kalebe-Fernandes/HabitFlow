namespace HabitFlow.Contracts.Settings
{
    /// <summary>
    /// Response DTO for user preferences (RF-030).
    /// </summary>
    public record UserPreferencesDto(
        string ThemeName,
        string? PrimaryColor,
        string? SecondaryColor,
        string? AccentColor,
        string FontSize, // Small, Medium, Large, ExtraLarge
        bool AnimationsEnabled,
        bool SoundEffectsEnabled,
        bool HapticFeedbackEnabled,
        bool FocusModeActive,
        VacationModeDto? VacationMode,
        NotificationPreferencesDto NotificationPreferences
    );
}
