using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Opah.BancoCarrefour.IoC.Modulos;

public class ApiModuloInicializar : IModuloInicializar
{
    #region métodos

    public void Inicializar(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddHealthChecks();
    }

    #endregion
}
