namespace HabitFlow.Contracts.Social
{
    /// <summary>
    /// Response DTO for groups (RF-022).
    /// </summary>
    public record GroupDto(
        Guid Id,
        string Name,
        string Description,
        string Type, // Private, Public
        Guid CreatorUserId,
        string CreatorName,
        int MemberCount,
        int MaxMembers,
        bool IsMember,
        string MemberRole, // Member, Moderator, Admin
        DateTime CreatedAt
    );
}
