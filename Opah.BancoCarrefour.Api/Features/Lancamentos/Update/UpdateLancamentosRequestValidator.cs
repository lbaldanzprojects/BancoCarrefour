using FluentValidation;

namespace Opah.BancoCarrefour.Api.Features.Lancamentos.Update;

public class UpdateLancamentosRequestValidator : AbstractValidator<UpdateLancamentosRequest>
{
    #region constructors

    public UpdateLancamentosRequestValidator()
    {
        RuleFor(p => p.Id)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Id do lançamento é obrigatório.");

        RuleFor(p => p.Valor)
            .GreaterThan(0)
            .WithMessage("Valor do lançamento deve ser maior que zero.");

        RuleFor(p => p.Tipo)
            .IsInEnum()
            .WithMessage("Tipo de lançamento inválido.");

        RuleFor(p => p.Descricao)
            .NotEmpty()
            .WithMessage("Descrição do lançamento é obrigatória.");

        RuleFor(p => p.DataLancamento)
            .NotEmpty()
            .WithMessage("Data do lançamento é obrigatória.");
    }

    #endregion
}
