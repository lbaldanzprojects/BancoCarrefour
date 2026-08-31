using AutoMapper;
using FluentValidation;
using Moq;
using Opah.BancoCarrefour.Application.Handlers.Lancamentos.Create;
using Opah.BancoCarrefour.Domain.Entidades;
using Opah.BancoCarrefour.Domain.Enums;
using Opah.BancoCarrefour.Domain.Ports.Repositorios;
using Xunit;

namespace Opah.BancoCarrefour.Tests.Handlers.Lancamentos;

public class CreateLancamentosHandlerTests
{
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ILancamentoOutboxCommand> _outboxCommandMock = new();

    private CreateLancamentosHandler CriarHandler()
        => new(_mapperMock.Object, _outboxCommandMock.Object);

    [Fact]
    public async Task Handle_ComandoValido_DeveGravarLancamentoComEventoDeOutboxERetornarResultadoMapeado()
    {
        var command = new CreateLancamentosCommand
        {
            Valor = 100m,
            Tipo = TipoLancamento.Credito,
            Descricao = "Venda balcão",
            DataLancamento = DateTime.UtcNow
        };

        var entidadeMapeada = new LancamentosEntity
        {
            Valor = command.Valor,
            Tipo = command.Tipo,
            Descricao = command.Descricao,
            DataLancamento = command.DataLancamento
        };

        var resultadoEsperado = new CreateLancamentosResult { Id = Guid.NewGuid(), Valor = command.Valor };

        _mapperMock.Setup(m => m.Map<LancamentosEntity>(command)).Returns(entidadeMapeada);

        _outboxCommandMock
            .Setup(o => o.CreateComEventoAsync(
                It.Is<LancamentosEntity>(e => e == entidadeMapeada),
                It.Is<OutboxMessageEntity>(m => m.EventType == "lancamento.registrado" && !string.IsNullOrEmpty(m.Payload)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(entidadeMapeada);

        _mapperMock.Setup(m => m.Map<CreateLancamentosResult>(entidadeMapeada)).Returns(resultadoEsperado);

        var handler = CriarHandler();

        var resultado = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(resultadoEsperado, resultado);
        Assert.NotEqual(Guid.Empty, entidadeMapeada.Id);
        _outboxCommandMock.Verify(o => o.CreateComEventoAsync(
            It.IsAny<LancamentosEntity>(),
            It.IsAny<OutboxMessageEntity>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ComandoInvalido_DeveLancarValidationExceptionSemPersistir()
    {
        var command = new CreateLancamentosCommand
        {
            Valor = 0m,
            Tipo = TipoLancamento.Credito,
            Descricao = "",
            DataLancamento = default
        };

        var handler = CriarHandler();

        await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));

        _outboxCommandMock.Verify(o => o.CreateComEventoAsync(
            It.IsAny<LancamentosEntity>(),
            It.IsAny<OutboxMessageEntity>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }
}
