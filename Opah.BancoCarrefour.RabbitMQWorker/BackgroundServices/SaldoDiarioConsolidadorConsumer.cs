using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Opah.BancoCarrefour.Domain.Enums;
using Opah.BancoCarrefour.Domain.Events;
using Opah.BancoCarrefour.Domain.Ports.Repositorios;
using Opah.BancoCarrefour.Messaging;
using Opah.BancoCarrefour.RabbitMQWorker.Resilience;
using Polly;
using Polly.CircuitBreaker;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Opah.BancoCarrefour.RabbitMQWorker.BackgroundServices;

public class SaldoDiarioConsolidadorConsumer : BackgroundService
{
    private const string ConsumerName = nameof(SaldoDiarioConsolidadorConsumer);

    private readonly IRabbitMqConnectionProvider _connectionProvider;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<SaldoDiarioConsolidadorConsumer> _logger;
    private readonly ResiliencePipeline _pipeline;

    public SaldoDiarioConsolidadorConsumer(
        IRabbitMqConnectionProvider connectionProvider,
        IServiceScopeFactory scopeFactory,
        ILogger<SaldoDiarioConsolidadorConsumer> logger)
    {
        _connectionProvider = connectionProvider;
        _scopeFactory = scopeFactory;
        _logger = logger;
        _pipeline = ResiliencePipelineFactory.CriarPadrao(logger, ConsumerName);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var connection = await _connectionProvider.GetConnectionAsync(stoppingToken);
        var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await RabbitMqTopologyInitializer.DeclareAsync(channel, stoppingToken);

        await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false, cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += (sender, ea) => ProcessarMensagemAsync(channel, ea, stoppingToken);

        await channel.BasicConsumeAsync(
            queue: RabbitMqTopology.SaldoDiarioConsolidacaoQueue,
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken);

        _logger.LogInformation("{Consumer} iniciado, aguardando mensagens na fila {Fila}.", ConsumerName, RabbitMqTopology.SaldoDiarioConsolidacaoQueue);

        await Task.Delay(Timeout.InfiniteTimeSpan, stoppingToken).ContinueWith(_ => { }, TaskScheduler.Default);
    }

    private async Task ProcessarMensagemAsync(IChannel channel, BasicDeliverEventArgs ea, CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var repositorio = scope.ServiceProvider.GetRequiredService<ISaldoDiarioConsolidacaoRepository>();

        try
        {
            var json = Encoding.UTF8.GetString(ea.Body.ToArray());
            var evento = JsonSerializer.Deserialize<LancamentoRegistradoEvent>(json)
                ?? throw new InvalidOperationException("Payload do evento lancamento.registrado veio nulo ou inválido.");

            await _pipeline.ExecuteAsync(async ct =>
            {
                var jaProcessado = await repositorio.EventoJaProcessadoAsync(evento.LancamentoId, ConsumerName, ct);
                if (jaProcessado)
                {
                    _logger.LogInformation("Evento {LancamentoId} já processado por {Consumer}; ignorando (idempotência).", evento.LancamentoId, ConsumerName);
                    return;
                }

                var deltaCreditos = evento.Tipo == TipoLancamento.Credito ? evento.Valor : 0m;
                var deltaDebitos = evento.Tipo == TipoLancamento.Debito ? evento.Valor : 0m;

                await repositorio.ConsolidarAsync(
                    evento.DataLancamento,
                    deltaCreditos,
                    deltaDebitos,
                    evento.LancamentoId,
                    ConsumerName,
                    ct);
            }, stoppingToken);

            await channel.BasicAckAsync(ea.DeliveryTag, multiple: false, cancellationToken: stoppingToken);
        }
        catch (BrokenCircuitException)
        {
            _logger.LogWarning(
                "Circuit breaker aberto: mensagem (deliveryTag {DeliveryTag}) enviada para a DLQ sem novas tentativas.",
                ea.DeliveryTag);

            await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false, cancellationToken: stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Falha ao consolidar mensagem (deliveryTag {DeliveryTag}) após esgotar retries. Enviando para a DLQ.",
                ea.DeliveryTag);

            await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false, cancellationToken: stoppingToken);
        }
    }
}
