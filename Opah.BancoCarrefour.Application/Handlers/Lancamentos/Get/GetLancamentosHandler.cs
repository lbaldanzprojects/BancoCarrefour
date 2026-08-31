using AutoMapper;
using Opah.BancoCarrefour.Domain.Ports.Repositorios;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Opah.BancoCarrefour.Domain.Entidades;

namespace Opah.BancoCarrefour.Application.Handlers.Lancamentos.Get;

public class GetLancamentosHandler : IRequestHandler<GetLancamentosCommand, GetLancamentosResult>
{
    #region atributos

    private readonly IMapper _mapper;
    private readonly IQuery<LancamentosEntity> _query;
    private readonly ILogger<GetLancamentosHandler> _logger;

    #endregion

    #region constructor

    public GetLancamentosHandler(
        IMapper mapper,
        IQuery<LancamentosEntity> query,
        ILogger<GetLancamentosHandler> logger)
    {
        _mapper = mapper;
        _query = query;
        _logger = logger;
    }

    #endregion

    #region métodos

    public async Task<GetLancamentosResult> Handle(GetLancamentosCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Validando os campos obrigatórios do comando.");

        var validator = new GetLancamentosCommandvalidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Iniciando o processo de busca do lançamento.");

        var entity = _mapper.Map<LancamentosEntity>(command);

        _logger.LogInformation("Buscando o lançamento registrado pelo Id informado.");
        var EntityResult = await _query.GetByIdAsync(entity.Id, cancellationToken);

        return _mapper.Map<GetLancamentosResult>(EntityResult);
    }

    #endregion
}
