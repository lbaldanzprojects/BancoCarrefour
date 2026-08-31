using Microsoft.AspNetCore.Builder;

namespace Opah.BancoCarrefour.IoC;

public interface IModuloInicializar
{
    #region métodos

    void Inicializar(WebApplicationBuilder builder);

    #endregion
}
