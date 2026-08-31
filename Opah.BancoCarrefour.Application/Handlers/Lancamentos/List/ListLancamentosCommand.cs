using Opah.BancoCarrefour.Application.Handlers.Lancamentos.Get;
using MediatR;

namespace Opah.BancoCarrefour.Application.Handlers.Lancamentos.List;

public record ListLancamentosCommand : IRequest<IEnumerable<GetLancamentosResult>> { }
