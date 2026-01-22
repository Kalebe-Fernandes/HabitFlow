namespace HabitFlow.Contracts.Templates
{
    /// <summary>
    /// Response DTO for guided journeys/programs (RF-026).
    /// </summary>
    public record JourneyDto(
        int Id,
        string Name,
        string Description,
        int DurationDays, // e.g., 7, 21, 30, 90
        string Category,
        string DifficultyLevel, // Beginner, Intermediate, Advanced
        string[] Goals,
        JourneyHabitDto[] Habits,
        int ParticipantCount,
        decimal AverageCompletionRate,
        bool IsFeatured
    );
}
