namespace HabitFlow.Contracts.Social
{
    /// <summary>
    /// Response DTO for leaderboards (RF-024).
    /// </summary>
    public record LeaderboardDto(
        string Type, // Daily, Weekly, Monthly, AllTime
        string Metric, // XP, Completions, Streak
        DateTime? PeriodStart,
        DateTime? PeriodEnd,
        LeaderboardEntryDto[] Entries,
        LeaderboardEntryDto? CurrentUserEntry
    );
}
