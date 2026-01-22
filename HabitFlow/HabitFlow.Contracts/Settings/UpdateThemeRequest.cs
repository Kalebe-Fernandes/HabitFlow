namespace HabitFlow.Contracts.Settings
{
    /// <summary>
    /// Request DTO for theme customization (RF-030).
    /// </summary>
    public record UpdateThemeRequest(
        string ThemeName, // Light, Dark, HighContrast, Custom
        string? PrimaryColor = null, // Hex color
        string? SecondaryColor = null,
        string? AccentColor = null
    );
}
