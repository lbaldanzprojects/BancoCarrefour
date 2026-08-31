using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Opah.BancoCarrefour.Application.Handlers.SaldoDiario.Get;
using Opah.BancoCarrefour.Application.Handlers.SaldoDiario.List;
using Opah.BancoCarrefour.Domain.Entidades;
using Opah.BancoCarrefour.Domain.Ports.Repositorios;
using Xunit;

namespace Opah.BancoCarrefour.Tests.Handlers.SaldoDiario;

public class ListSaldoDiarioHandlerTests
{
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ILogger<ListSaldoDiarioHandler>> _loggerMock = new();
    private readonly Mock<IQuery<SaldoDiarioEntity>> _queryMock = new();

    private ListSaldoDiarioHandler CriarHandler()
        => new(_mapperMock.Object, _loggerMock.Object, _queryMock.Object);

    [Fact]
    public async Task Handle_ExistemSaldosDiarios_DeveRetornarTodosMapeados()
    {
        var entidades = new List<SaldoDiarioEntity?>
        {
            new() { Id = Guid.NewGuid(), Data = DateTime.UtcNow.Date, TotalCreditos = 500m, TotalDebitos = 100m, SaldoConsolidado = 400m },
            new() { Id = Guid.NewGuid(), Data = DateTime.UtcNow.Date.AddDays(-1), TotalCreditos = 200m, TotalDebitos = 50m, SaldoConsolidado = 150m }
        };

        var resultadosEsperados = entidades.Select(e => new GetSaldoDiarioResult { Id = e!.Id, SaldoConsolidado = e.SaldoConsolidado }).ToList();

        _queryMock.Setup(q => q.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(entidades);
        _mapperMock.Setup(m => m.Map<IEnumerable<GetSaldoDiarioResult>>(entidades)).Returns(resultadosEsperados);

        var handler = CriarHandler();

        var resultado = await handler.Handle(new ListSaldoDiarioCommand(), CancellationToken.None);

        Assert.Equal(resultadosEsperados.Count, resultado.Count());
        Assert.Equal(resultadosEsperados, resultado);
    }

    [Fact]
    public async Task Handle_NaoExistemSaldosDiarios_DeveRetornarListaVazia()
    {
        _queryMock.Setup(q => q.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<SaldoDiarioEntity?>());
        _mapperMock.Setup(m => m.Map<IEnumerable<GetSaldoDiarioResult>>(It.IsAny<IEnumerable<SaldoDiarioEntity?>>()))
            .Returns(Enumerable.Empty<GetSaldoDiarioResult>());

        var handler = CriarHandler();

        var resultado = await handler.Handle(new ListSaldoDiarioCommand(), CancellationToken.None);

        Assert.Empty(resultado);
    }
}
