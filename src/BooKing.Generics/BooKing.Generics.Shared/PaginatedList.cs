namespace BooKing.Generics.Shared;
public class PaginatedList<T>
{
    public List<T> Items { get; private set; }
    public int PageIndex { get; private set; }
    public int TotalPages { get; private set; }

    public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        Items = items;
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
    }

    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;

    public static async Task<PaginatedList<T>> CreateAsync(int total, List<T> source, int pageIndex, int pageSize)
    {
        var count = total;
        var items = source;
        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }
}
