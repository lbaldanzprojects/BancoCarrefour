using MediatR;

namespace Opah.BancoCarrefour.Application.Handlers.Lancamentos.Delete;

public record DeleteLancamentosCommand : IRequest<DeleteLancamentosResult>
{
    #region construtores

    public DeleteLancamentosCommand(Guid id)
    {
        Id = id;
    }

    #endregion

    #region propriedades

    public Guid Id { get; }

    #endregion
}
