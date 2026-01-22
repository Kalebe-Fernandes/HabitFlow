namespace HabitFlow.Contracts.Templates
{
    /// <summary>
    /// Request DTO for browsing templates.
    /// </summary>
    public record BrowseTemplatesRequest(
        int? CategoryId = null,
        string? SearchTerm = null,
        string[]? Tags = null,
        bool? OnlyFeatured = null,
        string SortBy = "Popularity", // Popularity, Name, Newest
        int Page = 1,
        int PageSize = 20
    );
}
