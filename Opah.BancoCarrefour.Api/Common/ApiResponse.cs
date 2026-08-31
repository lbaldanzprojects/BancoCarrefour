using Opah.BancoCarrefour.Api.Common.Validation;

namespace Opah.BancoCarrefour.Api.Common;

public class ApiResponse
{
    #region propriedades

    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public IEnumerable<ValidationError> Errors { get; set; } = [];

    #endregion
}
