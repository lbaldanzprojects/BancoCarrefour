using System.Text.Json;
using AutoMapper;
using Opah.BancoCarrefour.Domain.Entidades;
using Opah.BancoCarrefour.Domain.Events;
using Opah.BancoCarrefour.Domain.Ports.Repositorios;
using FluentValidation;
using MediatR;

namespace Opah.BancoCarrefour.Application.Handlers.Lancamentos.Create;

public class CreateLancamentosHandler
    : IRequestHandler<CreateLancamentosCommand, CreateLancamentosResult>
{
    #region atributos

    private const string EventType = "lancamento.registrado";
    private const string RoutingKey = "lancamento.registrado";

    private readonly IMapper _mapper;
    private readonly ILancamentoOutboxCommand _outboxCommand;

    #endregion

    #region construtores

    public CreateLancamentosHandler(
        IMapper mapper,
        ILancamentoOutboxCommand outboxCommand)
    {
        _mapper = mapper;
        _outboxCommand = outboxCommand;
    }

    #endregion

    #region methods

    public async Task<CreateLancamentosResult> Handle(
        CreateLancamentosCommand command,
        CancellationToken cancellationToken)
    {
        var validator = new CreateLancamentosCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var entity = _mapper.Map<LancamentosEntity>(command);
        entity.Id = Guid.NewGuid();

        var evento = new LancamentoRegistradoEvent
        {
            LancamentoId = entity.Id,
            Valor = entity.Valor,
            Tipo = entity.Tipo,
            DataLancamento = entity.DataLancamento
        };

        var mensagemOutbox = new OutboxMessageEntity
        {
            Id = Guid.NewGuid(),
            EventType = EventType,
            RoutingKey = RoutingKey,
            Payload = JsonSerializer.Serialize(evento)
        };

        var result = await _outboxCommand.CreateComEventoAsync(entity, mensagemOutbox, cancellationToken);

        return _mapper.Map<CreateLancamentosResult>(result);
    }

    #endregion
}
