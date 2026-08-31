using FluentValidation.TestHelper;
using Opah.BancoCarrefour.Application.Handlers.Lancamentos.Update;
using Opah.BancoCarrefour.Domain.Enums;
using Xunit;

namespace Opah.BancoCarrefour.Tests.Validators.Lancamentos;

public class UpdateLancamentosCommandValidatorTests
{
    private readonly UpdateLancamentosCommandValidator _validator = new();

    [Fact]
    public void Validate_ComandoValido_NaoDeveGerarErros()
    {
        var command = new UpdateLancamentosCommand
        {
            Id = Guid.NewGuid(),
            Valor = 250.30m,
            Tipo = TipoLancamento.Debito,
            Descricao = "Pagamento de fornecedor",
            DataLancamento = DateTime.UtcNow
        };

        var resultado = _validator.TestValidate(command);

        resultado.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_IdVazio_DeveGerarErroEmId()
    {
        var command = new UpdateLancamentosCommand
        {
            Id = Guid.Empty,
            Valor = 100m,
            Tipo = TipoLancamento.Credito,
            Descricao = "Venda balcão",
            DataLancamento = DateTime.UtcNow
        };

        var resultado = _validator.TestValidate(command);

        resultado.ShouldHaveValidationErrorFor(c => c.Id);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-25)]
    public void Validate_ValorZeroOuNegativo_DeveGerarErroEmValor(decimal valor)
    {
        var command = new UpdateLancamentosCommand
        {
            Id = Guid.NewGuid(),
            Valor = valor,
            Tipo = TipoLancamento.Debito,
            Descricao = "Compra de insumos",
            DataLancamento = DateTime.UtcNow
        };

        var resultado = _validator.TestValidate(command);

        resultado.ShouldHaveValidationErrorFor(c => c.Valor);
    }

    [Fact]
    public void Validate_TipoInvalido_DeveGerarErroEmTipo()
    {
        var command = new UpdateLancamentosCommand
        {
            Id = Guid.NewGuid(),
            Valor = 50m,
            Tipo = (TipoLancamento)99,
            Descricao = "Lançamento qualquer",
            DataLancamento = DateTime.UtcNow
        };

        var resultado = _validator.TestValidate(command);

        resultado.ShouldHaveValidationErrorFor(c => c.Tipo);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Validate_DescricaoVazia_DeveGerarErroEmDescricao(string? descricao)
    {
        var command = new UpdateLancamentosCommand
        {
            Id = Guid.NewGuid(),
            Valor = 50m,
            Tipo = TipoLancamento.Credito,
            Descricao = descricao!,
            DataLancamento = DateTime.UtcNow
        };

        var resultado = _validator.TestValidate(command);

        resultado.ShouldHaveValidationErrorFor(c => c.Descricao);
    }

    [Fact]
    public void Validate_DataLancamentoNaoInformada_DeveGerarErroEmDataLancamento()
    {
        var command = new UpdateLancamentosCommand
        {
            Id = Guid.NewGuid(),
            Valor = 50m,
            Tipo = TipoLancamento.Credito,
            Descricao = "Lançamento sem data",
            DataLancamento = default
        };

        var resultado = _validator.TestValidate(command);

        resultado.ShouldHaveValidationErrorFor(c => c.DataLancamento);
    }
}
