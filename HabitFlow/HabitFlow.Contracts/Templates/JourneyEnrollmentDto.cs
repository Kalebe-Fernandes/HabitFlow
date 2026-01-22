namespace HabitFlow.Contracts.Templates
{
    /// <summary>
    /// Response DTO for journey enrollment.
    /// </summary>
    public record JourneyEnrollmentDto(
        long EnrollmentId,
        int JourneyId,
        string JourneyName,
        DateTime StartDate,
        DateTime EndDate,
        Guid[] CreatedHabitIds,
        string Status // Active, Completed, Abandoned
    );
}
