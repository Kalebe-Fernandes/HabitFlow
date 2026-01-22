namespace HabitFlow.Contracts.Social
{
    /// <summary>
    /// Request DTO for joining a challenge.
    /// </summary>
    public record JoinChallengeRequest(
        Guid ChallengeId
    );
}
