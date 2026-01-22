namespace HabitFlow.Contracts.Users
{
    /// <summary>
    /// Goal data for portability export.
    /// </summary>
    public record GoalDataDto(
        Guid Id,
        string Name,
        string? Description,
        decimal TargetValue,
        decimal CurrentValue,
        string Status,
        DateTime CreatedAt
    );
}
