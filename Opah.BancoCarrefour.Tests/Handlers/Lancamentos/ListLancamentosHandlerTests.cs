using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Opah.BancoCarrefour.Application.Handlers.Lancamentos.Get;
using Opah.BancoCarrefour.Application.Handlers.Lancamentos.List;
using Opah.BancoCarrefour.Domain.Entidades;
using Opah.BancoCarrefour.Domain.Enums;
using Opah.BancoCarrefour.Domain.Ports.Repositorios;
using Xunit;

namespace Opah.BancoCarrefour.Tests.Handlers.Lancamentos;

public class ListLancamentosHandlerTests
{
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ILogger<ListLancamentosHandler>> _loggerMock = new();
    private readonly Mock<IQuery<LancamentosEntity>> _queryMock = new();

    private ListLancamentosHandler CriarHandler()
        => new(_mapperMock.Object, _loggerMock.Object, _queryMock.Object);

    [Fact]
    public async Task Handle_ExistemLancamentos_DeveRetornarTodosMapeados()
    {
        var entidades = new List<LancamentosEntity?>
        {
            new() { Id = Guid.NewGuid(), Valor = 10m, Tipo = TipoLancamento.Credito, Descricao = "A", DataLancamento = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Valor = 20m, Tipo = TipoLancamento.Debito, Descricao = "B", DataLancamento = DateTime.UtcNow }
        };

        var resultadosEsperados = entidades.Select(e => new GetLancamentosResult { Id = e!.Id, Valor = e.Valor }).ToList();

        _queryMock.Setup(q => q.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(entidades);
        _mapperMock.Setup(m => m.Map<IEnumerable<GetLancamentosResult>>(entidades)).Returns(resultadosEsperados);

        var handler = CriarHandler();

        var resultado = await handler.Handle(new ListLancamentosCommand(), CancellationToken.None);

        Assert.Equal(resultadosEsperados.Count, resultado.Count());
        Assert.Equal(resultadosEsperados, resultado);
    }

    [Fact]
    public async Task Handle_NaoExistemLancamentos_DeveRetornarListaVazia()
    {
        _queryMock.Setup(q => q.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<LancamentosEntity?>());
        _mapperMock.Setup(m => m.Map<IEnumerable<GetLancamentosResult>>(It.IsAny<IEnumerable<LancamentosEntity?>>()))
            .Returns(Enumerable.Empty<GetLancamentosResult>());

        var handler = CriarHandler();

        var resultado = await handler.Handle(new ListLancamentosCommand(), CancellationToken.None);

        Assert.Empty(resultado);
    }
}
