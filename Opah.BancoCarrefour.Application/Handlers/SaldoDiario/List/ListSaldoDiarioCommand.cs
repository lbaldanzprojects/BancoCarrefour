using Opah.BancoCarrefour.Application.Handlers.SaldoDiario.Get;
using MediatR;

namespace Opah.BancoCarrefour.Application.Handlers.SaldoDiario.List;

public record ListSaldoDiarioCommand : IRequest<IEnumerable<GetSaldoDiarioResult>> { }
