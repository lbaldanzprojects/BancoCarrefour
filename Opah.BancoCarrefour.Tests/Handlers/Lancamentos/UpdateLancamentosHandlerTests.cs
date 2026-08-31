using AutoMapper;
using FluentValidation;
using Moq;
using Opah.BancoCarrefour.Application.Handlers.Lancamentos.Update;
using Opah.BancoCarrefour.Domain.Entidades;
using Opah.BancoCarrefour.Domain.Enums;
using Opah.BancoCarrefour.Domain.Ports.Repositorios;
using Xunit;

namespace Opah.BancoCarrefour.Tests.Handlers.Lancamentos;

public class UpdateLancamentosHandlerTests
{
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ICommand<LancamentosEntity>> _commandMock = new();

    private UpdateLancamentosHandler CriarHandler()
        => new(_mapperMock.Object, _commandMock.Object);

    [Fact]
    public async Task Handle_ComandoValido_DeveAtualizarDataAtualizacaoEChamarUpdateAsync()
    {
        var command = new UpdateLancamentosCommand
        {
            Id = Guid.NewGuid(),
            Valor = 200m,
            Tipo = TipoLancamento.Debito,
            Descricao = "Pagamento de fornecedor",
            DataLancamento = DateTime.UtcNow
        };

        var entidadeMapeada = new LancamentosEntity
        {
            Id = command.Id,
            Valor = command.Valor,
            Tipo = command.Tipo,
            Descricao = command.Descricao,
            DataLancamento = command.DataLancamento
        };

        var resultadoEsperado = new UpdateLancamentosResult { Id = command.Id, Valor = command.Valor };

        _mapperMock.Setup(m => m.Map<LancamentosEntity>(command)).Returns(entidadeMapeada);
        _commandMock.Setup(c => c.UpdateAsync(entidadeMapeada, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entidadeMapeada);
        _mapperMock.Setup(m => m.Map<UpdateLancamentosResult>(entidadeMapeada)).Returns(resultadoEsperado);

        var handler = CriarHandler();

        var resultado = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(resultadoEsperado, resultado);
        Assert.NotNull(entidadeMapeada.DataAtualizacao);
        _commandMock.Verify(c => c.UpdateAsync(entidadeMapeada, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ComandoInvalido_DeveLancarValidationExceptionSemPersistir()
    {
        var command = new UpdateLancamentosCommand
        {
            Id = Guid.Empty,
            Valor = -10m,
            Tipo = TipoLancamento.Credito,
            Descricao = "",
            DataLancamento = default
        };

        var handler = CriarHandler();

        await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));

        _commandMock.Verify(c => c.UpdateAsync(It.IsAny<LancamentosEntity>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
