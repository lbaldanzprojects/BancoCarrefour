using Opah.BancoCarrefour.SqlServer.Repository;
using Microsoft.EntityFrameworkCore;

namespace Opah.BancoCarrefour.Api.Extentions;

public static class MigrationExtension
{
    #region métodos

    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        var sqlServerContext = scope.ServiceProvider.GetRequiredService<SqlServerContext>();

        ApplyWithRetry(() => sqlServerContext.Database.Migrate(), "SQL Server");
    }

    #endregion

    #region private helpers

    private static void ApplyWithRetry(Action migrate, string databaseName)
    {
        const int maxRetries = 10;
        var delay = TimeSpan.FromSeconds(5);

        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                migrate();
                return;
            }
            catch (Exception ex)
            {
                if (attempt == maxRetries)
                    throw;
                Thread.Sleep(delay);
            }
        }
    }

    #endregion
}
