namespace HabitFlow.Contracts.Settings
{
    /// <summary>
    /// Request DTO for focus mode activation (RF-039).
    /// </summary>
    public record ActivateFocusModeRequest(
        bool Enable,
        DateTime? AutoDisableAt = null
    );
}
