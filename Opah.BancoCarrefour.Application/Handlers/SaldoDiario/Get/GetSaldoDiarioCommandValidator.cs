using FluentValidation;

namespace Opah.BancoCarrefour.Application.Handlers.SaldoDiario.Get;

public class GetSaldoDiarioCommandValidator : AbstractValidator<GetSaldoDiarioCommand>
{
    #region construtores

    public GetSaldoDiarioCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id do saldo diário é obrigatório.");
    }

    #endregion
}
