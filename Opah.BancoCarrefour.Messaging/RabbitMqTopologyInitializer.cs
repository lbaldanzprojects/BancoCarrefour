using RabbitMQ.Client;

namespace Opah.BancoCarrefour.Messaging;

public static class RabbitMqTopologyInitializer
{
    public static async Task DeclareAsync(IChannel channel, CancellationToken cancellationToken = default)
    {
        await channel.ExchangeDeclareAsync(
            exchange: RabbitMqTopology.EventosExchange,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false,
            cancellationToken: cancellationToken);

        await channel.ExchangeDeclareAsync(
            exchange: RabbitMqTopology.DeadLetterExchange,
            type: ExchangeType.Fanout,
            durable: true,
            autoDelete: false,
            cancellationToken: cancellationToken);

        await channel.QueueDeclareAsync(
            queue: RabbitMqTopology.SaldoDiarioConsolidacaoDlq,
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: cancellationToken);

        await channel.QueueBindAsync(
            queue: RabbitMqTopology.SaldoDiarioConsolidacaoDlq,
            exchange: RabbitMqTopology.DeadLetterExchange,
            routingKey: string.Empty,
            cancellationToken: cancellationToken);

        var argumentosFilaPrincipal = new Dictionary<string, object?>
        {
            ["x-dead-letter-exchange"] = RabbitMqTopology.DeadLetterExchange
        };

        await channel.QueueDeclareAsync(
            queue: RabbitMqTopology.SaldoDiarioConsolidacaoQueue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: argumentosFilaPrincipal,
            cancellationToken: cancellationToken);

        await channel.QueueBindAsync(
            queue: RabbitMqTopology.SaldoDiarioConsolidacaoQueue,
            exchange: RabbitMqTopology.EventosExchange,
            routingKey: RabbitMqTopology.LancamentoRegistradoRoutingKey,
            cancellationToken: cancellationToken);
    }
}
