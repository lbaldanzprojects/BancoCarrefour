using FluentValidation;

namespace Opah.BancoCarrefour.Api.Features.SaldoDiario.Get;

public class GetSaldoDiarioRequestValidator : AbstractValidator<GetSaldoDiarioRequest>
{
    #region constructors

    public GetSaldoDiarioRequestValidator()
    {
        RuleFor(p => p.Id)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithMessage("Id é obrigatório.");
    }

    #endregion
}
