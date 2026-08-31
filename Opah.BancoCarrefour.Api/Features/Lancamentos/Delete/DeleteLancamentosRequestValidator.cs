using FluentValidation;

namespace Opah.BancoCarrefour.Api.Features.Lancamentos.Delete;

public class DeleteLancamentosRequestValidator : AbstractValidator<DeleteLancamentosRequest>
{
    #region construtores

    public DeleteLancamentosRequestValidator()
    {
        RuleFor(p => p.Id)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("ID do usuário é obrigatório.");
    }

    #endregion
}
