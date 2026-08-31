using FluentValidation;

namespace Opah.BancoCarrefour.Application.Handlers.Lancamentos.Delete;

public class DeleteLancamentosCommandValidator : AbstractValidator<DeleteLancamentosCommand>
{
    #region construtores

    public DeleteLancamentosCommandValidator()
    {
        RuleFor(p => p.Id)
            .NotEmpty()
            .WithMessage("Id de usuário é obrigatório.");
    }

    #endregion
}
