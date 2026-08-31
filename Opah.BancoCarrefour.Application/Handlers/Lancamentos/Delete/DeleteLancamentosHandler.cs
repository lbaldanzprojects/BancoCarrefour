using AutoMapper;
using Opah.BancoCarrefour.Domain.Ports.Repositorios;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Opah.BancoCarrefour.Domain.Entidades;

namespace Opah.BancoCarrefour.Application.Handlers.Lancamentos.Delete;

public class DeleteLancamentosHandler
    : IRequestHandler<DeleteLancamentosCommand, DeleteLancamentosResult>
{
    #region atributos

    private readonly IMapper _mapper;
    private readonly ILogger<DeleteLancamentosHandler> _logger;
    private readonly ICommand<LancamentosEntity> _command;
    private readonly IQuery<LancamentosEntity> _query;

    #endregion

    #region construtores

    public DeleteLancamentosHandler(
        IMapper mapper,
        ILogger<DeleteLancamentosHandler> logger,
        ICommand<LancamentosEntity> command,
        IQuery<LancamentosEntity> query)
    {
        _mapper = mapper;
        _logger = logger;
        _command = command;
        _query = query;
    }

    #endregion

    public async Task<DeleteLancamentosResult> Handle(
        DeleteLancamentosCommand command,
        CancellationToken cancellationToken)
    {
        var validator = new DeleteLancamentosCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existing = await _query.GetByIdAsync(command.Id, cancellationToken);
        if (existing is null)
            throw new KeyNotFoundException($"Lançamento com Id {command.Id} não foi encontrado.");

        var entity = _mapper.Map<LancamentosEntity>(existing);
        await _command.DeleteAsync(entity, cancellationToken);

        return _mapper.Map<DeleteLancamentosResult>(entity);
    }
}
