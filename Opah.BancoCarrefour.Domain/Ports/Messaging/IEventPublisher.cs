namespace Opah.BancoCarrefour.Domain.Ports.Messaging;

public interface IEventPublisher
{
    Task PublishAsync(string payloadJson, string routingKey, CancellationToken cancellationToken = default);
}
