namespace HabitFlow.Contracts.Gamification
{
    // User Level DTOs
    public record UserLevelDto(
        Guid UserId,
        int CurrentLevel,
        long TotalXP,
        long XPForCurrentLevel,
        long XPForNextLevel,
        decimal ProgressToNextLevel // Percentage
    );
}
