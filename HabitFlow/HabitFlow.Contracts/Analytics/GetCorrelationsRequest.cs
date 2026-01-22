namespace HabitFlow.Contracts.Analytics
{
    /// <summary>
    /// Request DTO for getting correlations.
    /// </summary>
    public record GetCorrelationsRequest(
        DateTime? StartDate = null,
        DateTime? EndDate = null,
        string[]? Variables = null // If null, analyze all available variables
    );
}
