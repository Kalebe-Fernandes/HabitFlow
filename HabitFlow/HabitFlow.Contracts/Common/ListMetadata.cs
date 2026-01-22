namespace HabitFlow.Contracts.Common
{
    /// <summary>
    /// Metadata for list responses.
    /// </summary>
    public record ListMetadata(
        int TotalCount,
        int PageCount,
        int CurrentPage,
        int PageSize,
        bool HasNextPage,
        bool HasPreviousPage
    );
}
