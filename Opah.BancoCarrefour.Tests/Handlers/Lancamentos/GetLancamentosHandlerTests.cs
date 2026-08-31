using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using Opah.BancoCarrefour.Application.Handlers.Lancamentos.Get;
using Opah.BancoCarrefour.Domain.Entidades;
using Opah.BancoCarrefour.Domain.Enums;
using Opah.BancoCarrefour.Domain.Ports.Repositorios;
using Xunit;

namespace Opah.BancoCarrefour.Tests.Handlers.Lancamentos;

public class GetLancamentosHandlerTests
{
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IQuery<LancamentosEntity>> _queryMock = new();
    private readonly Mock<ILogger<GetLancamentosHandler>> _loggerMock = new();

    private GetLancamentosHandler CriarHandler()
        => new(_mapperMock.Object, _queryMock.Object, _loggerMock.Object);

    [Fact]
    public async Task Handle_LancamentoExistente_DeveRetornarResultadoMapeado()
    {
        var id = Guid.NewGuid();
        var command = new GetLancamentosCommand(id);
        var entidadeParaBusca = new LancamentosEntity { Id = id };

        var entidadeEncontrada = new LancamentosEntity
        {
            Id = id,
            Valor = 300m,
            Tipo = TipoLancamento.Credito,
            Descricao = "Recebimento",
            DataLancamento = DateTime.UtcNow
        };

        var resultadoEsperado = new GetLancamentosResult { Id = id, Valor = 300m };

        _mapperMock.Setup(m => m.Map<LancamentosEntity>(command)).Returns(entidadeParaBusca);
        _queryMock.Setup(q => q.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(entidadeEncontrada);
        _mapperMock.Setup(m => m.Map<GetLancamentosResult>(entidadeEncontrada)).Returns(resultadoEsperado);

        var handler = CriarHandler();

        var resultado = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(resultadoEsperado, resultado);
    }

    [Fact]
    public async Task Handle_ComandoComIdVazio_DeveLancarValidationException()
    {
        var command = new GetLancamentosCommand(Guid.Empty);
        var handler = CriarHandler();

        await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));

        _queryMock.Verify(q => q.GetByIdAsync(It.IsAny<Guid?>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
