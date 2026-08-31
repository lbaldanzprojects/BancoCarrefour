using FluentValidation;

namespace Opah.BancoCarrefour.Application.Handlers.Lancamentos.Get;

public class GetLancamentosCommandvalidator : AbstractValidator<GetLancamentosCommand>
{
	#region construtores

	public GetLancamentosCommandvalidator()
	{
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id da conta de usuário é obrigatório.");
    }

	#endregion
}
