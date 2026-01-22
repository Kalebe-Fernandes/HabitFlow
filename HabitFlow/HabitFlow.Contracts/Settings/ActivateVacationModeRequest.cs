namespace HabitFlow.Contracts.Settings
{
    /// <summary>
    /// Request DTO for activating vacation mode (RF-028).
    /// </summary>
    public record ActivateVacationModeRequest(
        DateTime StartDate,
        DateTime EndDate,
        Guid[]? SpecificHabitIds = null, // null = all habits
        string? Reason = null
    );
}
