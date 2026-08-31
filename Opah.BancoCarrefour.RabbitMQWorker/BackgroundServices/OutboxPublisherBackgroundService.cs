using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Opah.BancoCarrefour.Domain.Ports.Messaging;
using Opah.BancoCarrefour.Domain.Ports.Repositorios;
using Opah.BancoCarrefour.RabbitMQWorker.Resilience;
using Polly;
using Polly.CircuitBreaker;

namespace Opah.BancoCarrefour.RabbitMQWorker.BackgroundServices;

public class OutboxPublisherBackgroundService : BackgroundService
{
    private const int TamanhoLote = 20;
    private static readonly TimeSpan IntervaloDePolling = TimeSpan.FromSeconds(3);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OutboxPublisherBackgroundService> _logger;
    private readonly ResiliencePipeline _pipeline;

    public OutboxPublisherBackgroundService(
        IServiceScopeFactory scopeFactory,
        ILogger<OutboxPublisherBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _pipeline = ResiliencePipelineFactory.CriarPadrao(logger, nameof(OutboxPublisherBackgroundService));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("OutboxPublisherBackgroundService iniciado.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessarLotePendenteAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado no ciclo de publicação do Outbox.");
            }

            await Task.Delay(IntervaloDePolling, stoppingToken);
        }
    }

    private async Task ProcessarLotePendenteAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var outboxRepository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();
        var eventPublisher = scope.ServiceProvider.GetRequiredService<IEventPublisher>();

        var pendentes = await outboxRepository.ObterPendentesAsync(TamanhoLote, cancellationToken);

        foreach (var mensagem in pendentes)
        {
            try
            {
                await _pipeline.ExecuteAsync(async ct =>
                {
                    await eventPublisher.PublishAsync(mensagem.Payload, mensagem.RoutingKey, ct);
                }, cancellationToken);

                await outboxRepository.MarcarComoPublicadaAsync(mensagem.Id, cancellationToken);
            }
            catch (BrokenCircuitException)
            {
                _logger.LogWarning(
                    "Circuit breaker aberto: publicação de mensagens do Outbox pausada temporariamente. Mensagem {MensagemId} permanece pendente.",
                    mensagem.Id);

                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao publicar mensagem {MensagemId} do Outbox após esgotar as tentativas.", mensagem.Id);
                await outboxRepository.RegistrarFalhaEnvioAsync(mensagem.Id, ex.Message, cancellationToken);
            }
        }
    }
}
