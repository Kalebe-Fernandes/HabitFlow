namespace HabitFlow.Contracts.Templates
{
    /// <summary>
    /// Response DTO for habit templates (RF-025).
    /// </summary>
    public record HabitTemplateDto(
        int Id,
        string Name,
        string Description,
        string IconName,
        string ColorHex,
        string FrequencyType,
        int[]? DefaultDaysOfWeek,
        string TargetType,
        decimal? DefaultTargetValue,
        string? TargetUnit,
        int CategoryId,
        string CategoryName,
        int DefaultXpReward,
        int PopularityScore, // Number of times added by users
        string[] Tags,
        bool IsFeatured
    );
}
