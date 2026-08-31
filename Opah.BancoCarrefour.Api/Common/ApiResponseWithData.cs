using Opah.BancoCarrefour.Api.Common;

public class ApiResponseWithData<T> : ApiResponse
{
    #region propriedades

    public T Data { get; set; } = default!;

    #endregion
}
