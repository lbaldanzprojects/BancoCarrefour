using MediatR;

namespace Opah.BancoCarrefour.Application.Handlers.SaldoDiario.Get;

public record GetSaldoDiarioCommand : IRequest<GetSaldoDiarioResult>
{
    #region construtores

    public GetSaldoDiarioCommand(Guid id = default)
        => this.Id = id;

    #endregion

    #region propriedades

    public Guid Id { get; set; }

    #endregion
}
