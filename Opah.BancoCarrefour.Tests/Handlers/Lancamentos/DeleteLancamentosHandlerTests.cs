using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using Opah.BancoCarrefour.Application.Handlers.Lancamentos.Delete;
using Opah.BancoCarrefour.Domain.Entidades;
using Opah.BancoCarrefour.Domain.Enums;
using Opah.BancoCarrefour.Domain.Ports.Repositorios;
using Xunit;

namespace Opah.BancoCarrefour.Tests.Handlers.Lancamentos;

public class DeleteLancamentosHandlerTests
{
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ILogger<DeleteLancamentosHandler>> _loggerMock = new();
    private readonly Mock<ICommand<LancamentosEntity>> _commandMock = new();
    private readonly Mock<IQuery<LancamentosEntity>> _queryMock = new();

    private DeleteLancamentosHandler CriarHandler()
        => new(_mapperMock.Object, _loggerMock.Object, _commandMock.Object, _queryMock.Object);

    [Fact]
    public async Task Handle_LancamentoExistente_DeveExcluirERetornarResultadoMapeado()
    {
        var id = Guid.NewGuid();
        var command = new DeleteLancamentosCommand(id);

        var entidadeExistente = new LancamentosEntity
        {
            Id = id,
            Valor = 80m,
            Tipo = TipoLancamento.Debito,
            Descricao = "Compra",
            DataLancamento = DateTime.UtcNow
        };

        var resultadoEsperado = new DeleteLancamentosResult { Id = id };

        _queryMock.Setup(q => q.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(entidadeExistente);
        _mapperMock.Setup(m => m.Map<LancamentosEntity>(entidadeExistente)).Returns(entidadeExistente);
        _commandMock.Setup(c => c.DeleteAsync(entidadeExistente, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _mapperMock.Setup(m => m.Map<DeleteLancamentosResult>(entidadeExistente)).Returns(resultadoEsperado);

        var handler = CriarHandler();

        var resultado = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(resultadoEsperado, resultado);
        _commandMock.Verify(c => c.DeleteAsync(entidadeExistente, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_LancamentoInexistente_DeveLancarKeyNotFoundException()
    {
        var id = Guid.NewGuid();
        var command = new DeleteLancamentosCommand(id);

        _queryMock.Setup(q => q.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((LancamentosEntity?)null);

        var handler = CriarHandler();

        await Assert.ThrowsAsync<KeyNotFoundException>(() => handler.Handle(command, CancellationToken.None));

        _commandMock.Verify(c => c.DeleteAsync(It.IsAny<LancamentosEntity>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
