namespace HabitFlow.Contracts.Common
{
    /// <summary>
    /// Base request for paginated queries.
    /// </summary>
    public record PagedRequest
    {
        /// <summary>
        /// Page number (1-based).
        /// </summary>
        public int Page { get; init; } = 1;

        /// <summary>
        /// Number of items per page.
        /// </summary>
        public int PageSize { get; init; } = 20;

        /// <summary>
        /// Field to sort by.
        /// </summary>
        public string? SortBy { get; init; }

        /// <summary>
        /// Sort direction: "asc" or "desc".
        /// </summary>
        public string SortOrder { get; init; } = "asc";
    }
}
