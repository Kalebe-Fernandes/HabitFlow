namespace HabitFlow.Contracts.Templates
{
    /// <summary>
    /// Habit within a journey program.
    /// </summary>
    public record JourneyHabitDto(
        int TemplateId,
        string Name,
        string Description,
        int StartDay, // Day within journey to start (1-based)
        int DurationDays, // How many days this habit lasts
        bool IsRequired
    );
}
