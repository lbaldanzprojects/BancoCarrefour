namespace Opah.BancoCarrefour.Api.Common;

public class PaginatedResponse<T> : ApiResponseWithData<IEnumerable<T>>
{
    #region propriedades

    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }

    #endregion
}
