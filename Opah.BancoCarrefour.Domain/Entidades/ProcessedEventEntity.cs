using Opah.BancoCarrefour.Domain.Common;

namespace Opah.BancoCarrefour.Domain.Entidades;

public class ProcessedEventEntity : BaseEntidade
{
    #region propriedades

    public Guid EventoId { get; set; }

    public string ConsumerName { get; set; } = string.Empty;

    public DateTime ProcessadoEm { get; set; } = DateTime.UtcNow;

    #endregion
}
