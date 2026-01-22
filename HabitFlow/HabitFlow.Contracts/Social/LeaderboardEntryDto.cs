namespace HabitFlow.Contracts.Social
{
    /// <summary>
    /// Entry in a leaderboard.
    /// </summary>
    public record LeaderboardEntryDto(
        int Rank,
        Guid UserId,
        string UserName,
        string? AvatarUrl,
        int Value,
        int Level,
        bool IsCurrentUser
    );
}
