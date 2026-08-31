using System.Text;
using Microsoft.Extensions.Logging;
using Opah.BancoCarrefour.Domain.Ports.Messaging;
using RabbitMQ.Client;

namespace Opah.BancoCarrefour.Messaging;

public class RabbitMqEventPublisher : IEventPublisher
{
    #region atributos

    private readonly IRabbitMqConnectionProvider _connectionProvider;
    private readonly ILogger<RabbitMqEventPublisher> _logger;

    #endregion

    #region construtores

    public RabbitMqEventPublisher(
        IRabbitMqConnectionProvider connectionProvider,
        ILogger<RabbitMqEventPublisher> logger)
    {
        _connectionProvider = connectionProvider;
        _logger = logger;
    }

    #endregion

    #region métodos

    public async Task PublishAsync(string payloadJson, string routingKey, CancellationToken cancellationToken = default)
    {
        var connection = await _connectionProvider.GetConnectionAsync(cancellationToken);
        await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await RabbitMqTopologyInitializer.DeclareAsync(channel, cancellationToken);

        var corpo = Encoding.UTF8.GetBytes(payloadJson);

        var propriedades = new BasicProperties
        {
            DeliveryMode = DeliveryModes.Persistent,
            ContentType = "application/json"
        };

        await channel.BasicPublishAsync(
            exchange: RabbitMqTopology.EventosExchange,
            routingKey: routingKey,
            mandatory: false,
            basicProperties: propriedades,
            body: corpo,
            cancellationToken: cancellationToken);
    }

    #endregion
}
