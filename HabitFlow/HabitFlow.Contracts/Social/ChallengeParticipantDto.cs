namespace HabitFlow.Contracts.Social
{
    /// <summary>
    /// Participant in a challenge.
    /// </summary>
    public record ChallengeParticipantDto(
        Guid UserId,
        string UserName,
        string? AvatarUrl,
        int CurrentValue,
        int Rank,
        DateTime JoinedAt
    );
}
