namespace HabitFlow.Contracts.Common
{
    /// <summary>
    /// Generic paged list response.
    /// </summary>
    public record PagedListResponse<T>(
        T[] Items,
        ListMetadata Metadata
    );
}
