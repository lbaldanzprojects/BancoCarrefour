using Opah.BancoCarrefour.IoC.Modulos;
using Microsoft.AspNetCore.Builder;

namespace Opah.BancoCarrefour.IoC;

public static class DependencyInjection
{
    #region propriedades

    public static void RegisterDependencies(this WebApplicationBuilder builder)
    {
        new InfraModuloInicializar().Inicializar(builder);
        new ApiModuloInicializar().Inicializar(builder);
    }

    #endregion
}
