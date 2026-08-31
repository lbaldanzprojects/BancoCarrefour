using FluentValidation.Results;

namespace Opah.BancoCarrefour.Api.Common.Validation;

public class ValidationError
{
    #region constructors

    public static explicit operator ValidationError(ValidationFailure validationFailure)
    {
        return new ValidationError
        {
            Message = validationFailure.ErrorMessage ?? string.Empty,
        };
    }

    #endregion

    #region propriedades

    public string Message { get; private set; } = string.Empty;

    #endregion
}
