namespace HabitFlow.Contracts.Settings
{
    /// <summary>
    /// Response DTO for vacation mode status.
    /// </summary>
    public record VacationModeDto(
        bool IsActive,
        DateTime? StartDate,
        DateTime? EndDate,
        int DaysRemaining,
        Guid[] AffectedHabitIds,
        string Status // Scheduled, Active, Ended
    );
}
