namespace HabitFlow.Contracts.Templates
{
    /// <summary>
    /// Request DTO for starting a journey (RF-027).
    /// </summary>
    public record StartJourneyRequest(
        int JourneyId,
        DateTime StartDate
    );
}
