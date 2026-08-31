using MediatR;

namespace Opah.BancoCarrefour.Application.Handlers.Lancamentos.Get;

public record GetLancamentosCommand : IRequest<GetLancamentosResult>
{
    #region construtores

    public GetLancamentosCommand(Guid id = default)
        => this.Id = id;

    #endregion

    #region propriedades

    public Guid Id { get; set; }

    #endregion
}
