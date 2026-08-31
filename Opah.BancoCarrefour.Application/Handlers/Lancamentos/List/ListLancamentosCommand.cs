using Opah.BancoCarrefour.Application.Handlers.Lancamentos.Get;
using Opah.BancoCarrefour.Domain.Enums;
using MediatR;

namespace Opah.BancoCarrefour.Application.Handlers.Lancamentos.List;

public record ListLancamentosCommand : IRequest<IEnumerable<GetLancamentosResult>>
{
    public DateTime? DataInicio { get; init; }
    public DateTime? DataFim { get; init; }
    public TipoLancamento? Tipo { get; init; }
}
