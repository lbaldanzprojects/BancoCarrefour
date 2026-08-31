using Opah.BancoCarrefour.Domain.Entidades;
using Opah.BancoCarrefour.Domain.Ports.Repositorios;
using Opah.BancoCarrefour.SqlServer.Repository;
using Opah.BancoCarrefour.SqlServer.Repository.CQRS.Commands;
using Opah.BancoCarrefour.SqlServer.Repository.CQRS.Queries;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Opah.BancoCarrefour.IoC.Modulos;

public class InfraModuloInicializar : IModuloInicializar
{
    public void Inicializar(WebApplicationBuilder builder)
    {
        var sqlServerConnString = builder.Configuration.GetConnectionString("SqlServerConn");

        builder.Services.AddDbContext<SqlServerContext>(options =>
            options.UseSqlServer(
                sqlServerConnString,
                sql =>
                {
                    sql.MigrationsAssembly("Opah.BancoCarrefour.SqlServer.Repository");
                    sql.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null);
                }));

        builder.Services.AddScoped<ICommand<LancamentosEntity>, SqlLancamentosCommand>();
        builder.Services.AddScoped<IQuery<LancamentosEntity>, SqlLancamentosQuery>();

        builder.Services.AddScoped<ICommand<SaldoDiarioEntity>, SqlSaldoDiarioCommand>();
        builder.Services.AddScoped<IQuery<SaldoDiarioEntity>, SqlSaldoDiarioQuery>();

        builder.Services.AddScoped<ILancamentoOutboxCommand, SqlLancamentoOutboxCommand>();
    }
}
