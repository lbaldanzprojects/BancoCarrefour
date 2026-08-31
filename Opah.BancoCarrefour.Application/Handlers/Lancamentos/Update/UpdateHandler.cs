using AutoMapper;
using Opah.BancoCarrefour.Domain.Ports.Repositorios;
using FluentValidation;
using MediatR;
using Opah.BancoCarrefour.Domain.Entidades;

namespace Opah.BancoCarrefour.Application.Handlers.Lancamentos.Update;

public class UpdateLancamentosHandler : IRequestHandler<UpdateLancamentosCommand, UpdateLancamentosResult>
{
    #region atributos

    private readonly IMapper _mapper;
    private readonly ICommand<LancamentosEntity> _command;

    #endregion

    #region constructor

    public UpdateLancamentosHandler(IMapper mapper, ICommand<LancamentosEntity> command)
    {
        _mapper = mapper;
        _command = command;
    }

    #endregion

    public async Task<UpdateLancamentosResult> Handle(UpdateLancamentosCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateLancamentosCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var entity = _mapper.Map<LancamentosEntity>(command);
        entity.DataAtualizacao = DateTime.UtcNow;

        var result = await _command.UpdateAsync(entity, cancellationToken);

        return _mapper.Map<UpdateLancamentosResult>(result);
    }
}
