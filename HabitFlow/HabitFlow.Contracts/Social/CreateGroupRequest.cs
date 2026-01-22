namespace HabitFlow.Contracts.Social
{
    /// <summary>
    /// Request DTO for creating a group.
    /// </summary>
    public record CreateGroupRequest(
        string Name,
        string Description,
        string Type, // Private, Public
        int MaxMembers = 100
    );
}
