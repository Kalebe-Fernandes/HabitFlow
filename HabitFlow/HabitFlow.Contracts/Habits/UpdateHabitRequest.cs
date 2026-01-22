namespace HabitFlow.Contracts.Habits
{
    public record UpdateHabitRequest(
        string Name,
        string? Description,
        string IconName,
        string ColorHex,
        int CategoryId
    );
}
