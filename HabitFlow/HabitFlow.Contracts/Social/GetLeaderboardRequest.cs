namespace HabitFlow.Contracts.Social
{
    /// <summary>
    /// Request DTO for getting leaderboard.
    /// </summary>
    public record GetLeaderboardRequest(
        string Type = "Weekly", // Daily, Weekly, Monthly, AllTime
        string Metric = "XP", // XP, Completions, Streak
        Guid? GroupId = null, // null for global leaderboard
        int Top = 100
    );
}
