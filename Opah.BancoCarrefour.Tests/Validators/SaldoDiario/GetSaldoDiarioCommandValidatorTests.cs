using FluentValidation.TestHelper;
using Opah.BancoCarrefour.Application.Handlers.SaldoDiario.Get;
using Xunit;

namespace Opah.BancoCarrefour.Tests.Validators.SaldoDiario;

public class GetSaldoDiarioCommandValidatorTests
{
    private readonly GetSaldoDiarioCommandValidator _validator = new();

    [Fact]
    public void Validate_ComandoValido_NaoDeveGerarErros()
    {
        var command = new GetSaldoDiarioCommand(Guid.NewGuid());

        var resultado = _validator.TestValidate(command);

        resultado.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_IdVazio_DeveGerarErroEmId()
    {
        var command = new GetSaldoDiarioCommand(Guid.Empty);

        var resultado = _validator.TestValidate(command);

        resultado.ShouldHaveValidationErrorFor(c => c.Id);
    }
}
