namespace HabitFlow.Contracts.Settings
{
    /// <summary>
    /// Request DTO for using a pass (RF-029).
    /// </summary>
    public record UsePassRequest(
        Guid HabitId,
        DateTime Date,
        string? Reason = null
    );
}
