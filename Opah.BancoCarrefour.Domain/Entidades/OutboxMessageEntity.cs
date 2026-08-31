using Opah.BancoCarrefour.Domain.Common;

namespace Opah.BancoCarrefour.Domain.Entidades;

public class OutboxMessageEntity : BaseEntidade
{
    #region propriedades

    public string EventType { get; set; } = string.Empty;

    public string RoutingKey { get; set; } = string.Empty;

    public string Payload { get; set; } = string.Empty;

    public DateTime? ProcessedAt { get; set; }

    public int TentativasEnvio { get; set; }

    public string? UltimoErro { get; set; }

    #endregion
}
