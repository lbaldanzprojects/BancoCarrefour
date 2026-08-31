using Opah.BancoCarrefour.Domain.Enums;

namespace Opah.BancoCarrefour.Domain.Events;

public record LancamentoRegistradoEvent
{
    public Guid LancamentoId { get; init; }
    public decimal Valor { get; init; }
    public TipoLancamento Tipo { get; init; }
    public DateTime DataLancamento { get; init; }
    public DateTime OcorridoEm { get; init; } = DateTime.UtcNow;
}
