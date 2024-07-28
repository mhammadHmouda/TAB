namespace TAB.Application.Core.Contracts;

public class PagedList<T>
{
    private PagedList(List<T> items, int page, int pageSize, int totalCount)
    {
        Items = items;
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
        HasNextPage = Page * PageSize < TotalCount;
        HasPreviousPage = page > 1;
    }

    public List<T> Items { get; }
    public int Page { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public bool HasNextPage { get; }
    public bool HasPreviousPage { get; }

    public static PagedList<T> Create(List<T> items, int page, int pageSize, int totalCount) =>
        new(items, page, pageSize, totalCount);
}
