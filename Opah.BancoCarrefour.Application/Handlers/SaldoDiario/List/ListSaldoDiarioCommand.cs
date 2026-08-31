using Opah.BancoCarrefour.Application.Handlers.SaldoDiario.Get;
using MediatR;

namespace Opah.BancoCarrefour.Application.Handlers.SaldoDiario.List;

public record ListSaldoDiarioCommand : IRequest<IEnumerable<GetSaldoDiarioResult>>
{
    public DateTime? DataInicio { get; init; }
    public DateTime? DataFim { get; init; }
}
