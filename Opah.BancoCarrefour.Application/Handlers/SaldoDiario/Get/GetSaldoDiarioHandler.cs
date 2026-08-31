using AutoMapper;
using Opah.BancoCarrefour.Domain.Ports.Repositorios;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Opah.BancoCarrefour.Domain.Entidades;

namespace Opah.BancoCarrefour.Application.Handlers.SaldoDiario.Get;

public class GetSaldoDiarioHandler : IRequestHandler<GetSaldoDiarioCommand, GetSaldoDiarioResult>
{
    #region atributos

    private readonly IMapper _mapper;
    private readonly IQuery<SaldoDiarioEntity> _query;
    private readonly ILogger<GetSaldoDiarioHandler> _logger;

    #endregion

    #region constructor

    public GetSaldoDiarioHandler(
        IMapper mapper,
        IQuery<SaldoDiarioEntity> query,
        ILogger<GetSaldoDiarioHandler> logger)
    {
        _mapper = mapper;
        _query = query;
        _logger = logger;
    }

    #endregion

    #region métodos

    public async Task<GetSaldoDiarioResult> Handle(GetSaldoDiarioCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Validando os campos obrigatórios do comando.");

        var validator = new GetSaldoDiarioCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Iniciando o processo de busca do saldo diário.");

        var entity = _mapper.Map<SaldoDiarioEntity>(command);

        _logger.LogInformation("Buscando o saldo diário registrado pelo Id informado.");
        var EntityResult = await _query.GetByIdAsync(entity.Id, cancellationToken);

        return _mapper.Map<GetSaldoDiarioResult>(EntityResult);
    }

    #endregion
}
