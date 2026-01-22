namespace HabitFlow.Contracts.Social
{
    /// <summary>
    /// Response DTO for group members.
    /// </summary>
    public record GroupMemberDto(
        Guid UserId,
        string UserName,
        string? AvatarUrl,
        string Role, // Member, Moderator, Admin
        int Level,
        long TotalXP,
        DateTime JoinedAt
    );
}
