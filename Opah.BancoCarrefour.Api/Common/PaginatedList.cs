public class PaginatedList<T> : List<T>
{
    #region constructors

    public PaginatedList(List<T> items, int count, int pageNumber = 1, int pageSize = 10)
    {
        TotalCount = count;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        Items = items;

        AddRange(Items);
    }

    #endregion

    #region propriedades

    public static async Task<PaginatedList<T>> GetAsync(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = source.Count();
        var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        return new PaginatedList<T>(items, count, pageNumber, pageSize);
    }

    #endregion

    #region propriedades

    public int CurrentPage { get; private set; }

    public int TotalPages { get; private set; }

    public int PageSize { get; private set; }

    public int TotalCount { get; private set; }

    public List<T> Items { get; private set; }

    public bool HasPrevious => CurrentPage > 1;

    public bool HasNext => CurrentPage < TotalPages;

    #endregion
}
