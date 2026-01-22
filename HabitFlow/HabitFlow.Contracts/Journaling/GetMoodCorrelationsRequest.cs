namespace HabitFlow.Contracts.Journaling
{
    /// <summary>
    /// Request DTO for mood/energy correlations.
    /// </summary>
    public record GetMoodCorrelationsRequest(
        DateTime? StartDate = null,
        DateTime? EndDate = null,
        Guid? SpecificHabitId = null
    );
}
