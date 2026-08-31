using FluentValidation;

namespace Opah.BancoCarrefour.Application.Handlers.Lancamentos.Update;

public class UpdateLancamentosCommandValidator : AbstractValidator<UpdateLancamentosCommand>
{
    #region construtores

    public UpdateLancamentosCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id do lançamento é obrigatório.");

        RuleFor(x => x.Valor)
            .GreaterThan(0)
            .WithMessage("Valor do lançamento deve ser maior que zero.");

        RuleFor(x => x.Tipo)
            .IsInEnum()
            .WithMessage("Tipo de lançamento inválido.");

        RuleFor(x => x.Descricao)
            .NotEmpty()
            .WithMessage("Descrição do lançamento é obrigatória.");

        RuleFor(x => x.DataLancamento)
            .NotEmpty()
            .WithMessage("Data do lançamento é obrigatória.");
    }

    #endregion
}
