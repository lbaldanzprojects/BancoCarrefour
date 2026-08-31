using AutoMapper;
using Opah.BancoCarrefour.Application.Handlers.SaldoDiario.Get;
using Opah.BancoCarrefour.Domain.Ports.Repositorios;
using MediatR;
using Microsoft.Extensions.Logging;
using Opah.BancoCarrefour.Domain.Entidades;

namespace Opah.BancoCarrefour.Application.Handlers.SaldoDiario.List;

public class ListSaldoDiarioHandler : IRequestHandler<ListSaldoDiarioCommand, IEnumerable<GetSaldoDiarioResult>>
{
    #region atributos

    private readonly IMapper _mapper;
    private readonly ILogger<ListSaldoDiarioHandler> _logger;
    private readonly IQuery<SaldoDiarioEntity> _query;

    #endregion

    #region constructors

    public ListSaldoDiarioHandler(IMapper mapper,
        ILogger<ListSaldoDiarioHandler> logger,
        IQuery<SaldoDiarioEntity> query)
    {
        _mapper = mapper;
        _logger = logger;
        _query = query;
    }

    #endregion

    #region propriedades

    public async Task<IEnumerable<GetSaldoDiarioResult>> Handle(ListSaldoDiarioCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando o processo de listagem dos saldos diários.");

        var entity = await _query.GetAllAsync(cancellationToken);
        var result = _mapper.Map<IEnumerable<GetSaldoDiarioResult>>(entity);

        _logger.LogInformation("Listagem de saldos diários concluída com {Count} registros encontrados.", result.Count());

        return result;
    }

    #endregion
}
