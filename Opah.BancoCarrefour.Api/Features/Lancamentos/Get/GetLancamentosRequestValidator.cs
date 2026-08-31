using FluentValidation;

namespace Opah.BancoCarrefour.Api.Features.Lancamentos.Get;

public class GetLancamentosRequestValidator : AbstractValidator<GetLancamentosRequest>
{
    #region constructors

    public GetLancamentosRequestValidator()
    {
        RuleFor(p => p.Id)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithMessage("Id é obrigatório.");
    }

    #endregion
}
