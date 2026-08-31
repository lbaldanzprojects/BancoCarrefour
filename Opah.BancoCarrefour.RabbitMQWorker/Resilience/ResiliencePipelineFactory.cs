using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace Opah.BancoCarrefour.RabbitMQWorker.Resilience;

public static class ResiliencePipelineFactory
{
    public static ResiliencePipeline CriarPadrao(ILogger logger, string nomeOperacao)
    {
        return new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                ShouldHandle = new PredicateBuilder().Handle<Exception>(ex => ex is not BrokenCircuitException),
                MaxRetryAttempts = 3,
                BackoffType = DelayBackoffType.Exponential,
                Delay = TimeSpan.FromSeconds(1),
                OnRetry = args =>
                {
                    logger.LogWarning(
                        args.Outcome.Exception,
                        "[{Operacao}] Tentativa {Tentativa} falhou. Nova tentativa em {Delay}.",
                        nomeOperacao,
                        args.AttemptNumber + 1,
                        args.RetryDelay);

                    return ValueTask.CompletedTask;
                }
            })
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                ShouldHandle = new PredicateBuilder().Handle<Exception>(),
                FailureRatio = 0.5,
                SamplingDuration = TimeSpan.FromSeconds(30),
                MinimumThroughput = 5,
                BreakDuration = TimeSpan.FromSeconds(30),
                OnOpened = args =>
                {
                    logger.LogError(
                        "[{Operacao}] Circuit breaker ABERTO por {BreakDuration}. Falhas consecutivas acima do limite.",
                        nomeOperacao,
                        args.BreakDuration);

                    return ValueTask.CompletedTask;
                },
                OnClosed = args =>
                {
                    logger.LogInformation("[{Operacao}] Circuit breaker FECHADO. Operação normalizada.", nomeOperacao);
                    return ValueTask.CompletedTask;
                },
                OnHalfOpened = args =>
                {
                    logger.LogInformation("[{Operacao}] Circuit breaker em teste (half-open).", nomeOperacao);
                    return ValueTask.CompletedTask;
                }
            })
            .Build();
    }
}
