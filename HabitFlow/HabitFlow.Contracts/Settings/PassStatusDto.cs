namespace HabitFlow.Contracts.Settings
{
    /// <summary>
    /// Response DTO for pass usage status.
    /// </summary>
    public record PassStatusDto(
        int AvailablePasses,
        int TotalPasses,
        int UsedThisMonth,
        DateTime NextReplenishmentDate,
        PassUsageDto[] RecentUsages
    );
}
