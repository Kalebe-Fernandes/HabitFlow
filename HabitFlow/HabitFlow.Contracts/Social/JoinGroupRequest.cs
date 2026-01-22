namespace HabitFlow.Contracts.Social
{
    /// <summary>
    /// Request DTO for joining a group.
    /// </summary>
    public record JoinGroupRequest(
        Guid GroupId,
        string? InviteCode = null
    );
}
