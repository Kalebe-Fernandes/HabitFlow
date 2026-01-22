namespace HabitFlow.Contracts.Templates
{
    /// <summary>
    /// Request DTO for browsing journeys.
    /// </summary>
    public record BrowseJourneysRequest(
        string? Category = null,
        string? DifficultyLevel = null,
        int? MaxDurationDays = null,
        bool? OnlyFeatured = null,
        int Page = 1,
        int PageSize = 20
    );
}
