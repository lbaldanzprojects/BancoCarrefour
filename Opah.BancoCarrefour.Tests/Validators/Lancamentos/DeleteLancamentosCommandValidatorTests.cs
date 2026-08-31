using FluentValidation.TestHelper;
using Opah.BancoCarrefour.Application.Handlers.Lancamentos.Delete;
using Xunit;

namespace Opah.BancoCarrefour.Tests.Validators.Lancamentos;

public class DeleteLancamentosCommandValidatorTests
{
    private readonly DeleteLancamentosCommandValidator _validator = new();

    [Fact]
    public void Validate_ComandoValido_NaoDeveGerarErros()
    {
        var command = new DeleteLancamentosCommand(Guid.NewGuid());

        var resultado = _validator.TestValidate(command);

        resultado.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_IdVazio_DeveGerarErroEmId()
    {
        var command = new DeleteLancamentosCommand(Guid.Empty);

        var resultado = _validator.TestValidate(command);

        resultado.ShouldHaveValidationErrorFor(c => c.Id);
    }
}
