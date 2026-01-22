namespace HabitFlow.Contracts.Social
{
    /// <summary>
    /// Request DTO for creating a challenge.
    /// </summary>
    public record CreateChallengeRequest(
        string Name,
        string Description,
        Guid? GroupId, // null for public challenge
        int TargetValue,
        string Unit,
        DateTime StartDate,
        DateTime EndDate
    );
}
