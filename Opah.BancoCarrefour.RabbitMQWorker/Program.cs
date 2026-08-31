using Microsoft.EntityFrameworkCore;
using Opah.BancoCarrefour.Domain.Ports.Repositorios;
using Opah.BancoCarrefour.Messaging;
using Opah.BancoCarrefour.RabbitMQWorker.BackgroundServices;
using Opah.BancoCarrefour.SqlServer.Repository;
using Opah.BancoCarrefour.SqlServer.Repository.CQRS.Commands;

var builder = Host.CreateApplicationBuilder(args);

var sqlServerConnString = builder.Configuration.GetConnectionString("SqlServerConn");

builder.Services.AddDbContext<SqlServerContext>(options =>
    options.UseSqlServer(
        sqlServerConnString,
        sql =>
        {
            sql.MigrationsAssembly("Opah.BancoCarrefour.SqlServer.Repository");
            sql.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null);
        }));

builder.Services.AddScoped<IOutboxRepository, SqlOutboxRepository>();
builder.Services.AddScoped<ISaldoDiarioConsolidacaoRepository, SqlSaldoDiarioConsolidacaoRepository>();

builder.Services.AddRabbitMqMessaging(builder.Configuration);

builder.Services.AddHostedService<OutboxPublisherBackgroundService>();
builder.Services.AddHostedService<SaldoDiarioConsolidadorConsumer>();

var host = builder.Build();
host.Run();
