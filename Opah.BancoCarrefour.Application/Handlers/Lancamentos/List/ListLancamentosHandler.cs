using AutoMapper;
using Opah.BancoCarrefour.Application.Handlers.Lancamentos.Get;
using Opah.BancoCarrefour.Domain.Ports.Repositorios;
using MediatR;
using Microsoft.Extensions.Logging;
using Opah.BancoCarrefour.Domain.Entidades;

namespace Opah.BancoCarrefour.Application.Handlers.Lancamentos.List;

public class ListLancamentosHandler : IRequestHandler<ListLancamentosCommand, IEnumerable<GetLancamentosResult>>
{
    #region atributos

    private readonly IMapper _mapper;
    private readonly ILogger<ListLancamentosHandler> _logger;
    private readonly IQuery<LancamentosEntity> _query;

    #endregion

    #region constructors

    public ListLancamentosHandler(IMapper mapper,
        ILogger<ListLancamentosHandler> logger,
        IQuery<LancamentosEntity> query)
    {
        _mapper = mapper;
        _logger = logger;
        _query = query;
    }

    #endregion

    #region propriedades

    public async Task<IEnumerable<GetLancamentosResult>> Handle(ListLancamentosCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando o processo de listagem de lançamentos.");

        var entity = await _query.GetAllAsync(cancellationToken);
        var result = _mapper.Map<IEnumerable<GetLancamentosResult>>(entity);

        _logger.LogInformation("Listagem de lançamentos concluída com {Count} registros encontrados.", result.Count());

        return result;
    }

    #endregion
}
