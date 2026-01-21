namespace HabitFlow.Aplicacao.Common.Models
{
    public class PagedResult<T>(IEnumerable<T> items, int totalCount, int page, int pageSize)
    {
        public IEnumerable<T> Items { get; } = items;
        public int TotalCount { get; } = totalCount;
        public int Page { get; } = page;
        public int PageSize { get; } = pageSize;
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public bool HasPreviousPage => Page > 1;
        public bool HasNextPage => Page < TotalPages;
    }
}
