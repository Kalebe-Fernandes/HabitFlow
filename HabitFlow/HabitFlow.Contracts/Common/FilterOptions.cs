namespace HabitFlow.Contracts.Common
{
    /// <summary>
    /// Filter options for list queries.
    /// </summary>
    public record FilterOptions(
        string? SearchTerm = null,
        DateTime? StartDate = null,
        DateTime? EndDate = null,
        string? SortBy = null,
        string SortDirection = "Asc", // Asc, Desc
        Dictionary<string, string>? CustomFilters = null
    );
}
