using FluentValidation.TestHelper;
using Opah.BancoCarrefour.Application.Handlers.Lancamentos.Get;
using Xunit;

namespace Opah.BancoCarrefour.Tests.Validators.Lancamentos;

public class GetLancamentosCommandValidatorTests
{
    private readonly GetLancamentosCommandvalidator _validator = new();

    [Fact]
    public void Validate_ComandoValido_NaoDeveGerarErros()
    {
        var command = new GetLancamentosCommand(Guid.NewGuid());

        var resultado = _validator.TestValidate(command);

        resultado.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_IdVazio_DeveGerarErroEmId()
    {
        var command = new GetLancamentosCommand(Guid.Empty);

        var resultado = _validator.TestValidate(command);

        resultado.ShouldHaveValidationErrorFor(c => c.Id);
    }
}
