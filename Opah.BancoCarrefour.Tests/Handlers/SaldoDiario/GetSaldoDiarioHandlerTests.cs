using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Opah.BancoCarrefour.Application.Handlers.SaldoDiario.Get;
using Opah.BancoCarrefour.Domain.Entidades;
using Opah.BancoCarrefour.Domain.Ports.Repositorios;
using Xunit;

namespace Opah.BancoCarrefour.Tests.Handlers.SaldoDiario;

public class GetSaldoDiarioHandlerTests
{
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IQuery<SaldoDiarioEntity>> _queryMock = new();
    private readonly Mock<ILogger<GetSaldoDiarioHandler>> _loggerMock = new();

    private GetSaldoDiarioHandler CriarHandler()
        => new(_mapperMock.Object, _queryMock.Object, _loggerMock.Object);

    [Fact]
    public async Task Handle_SaldoDiarioExistente_DeveRetornarResultadoMapeado()
    {
        var id = Guid.NewGuid();
        var command = new GetSaldoDiarioCommand(id);
        var entidadeParaBusca = new SaldoDiarioEntity { Id = id };

        var entidadeEncontrada = new SaldoDiarioEntity
        {
            Id = id,
            Data = DateTime.UtcNow.Date,
            TotalCreditos = 500m,
            TotalDebitos = 100m,
            SaldoConsolidado = 400m
        };

        var resultadoEsperado = new GetSaldoDiarioResult { Id = id, SaldoConsolidado = 400m };

        _mapperMock.Setup(m => m.Map<SaldoDiarioEntity>(command)).Returns(entidadeParaBusca);
        _queryMock.Setup(q => q.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(entidadeEncontrada);
        _mapperMock.Setup(m => m.Map<GetSaldoDiarioResult>(entidadeEncontrada)).Returns(resultadoEsperado);

        var handler = CriarHandler();

        var resultado = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(resultadoEsperado, resultado);
    }

    [Fact]
    public async Task Handle_ComandoComIdVazio_DeveLancarValidationException()
    {
        var command = new GetSaldoDiarioCommand(Guid.Empty);
        var handler = CriarHandler();

        await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => handler.Handle(command, CancellationToken.None));

        _queryMock.Verify(q => q.GetByIdAsync(It.IsAny<Guid?>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
