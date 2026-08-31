using Opah.BancoCarrefour.Domain.Common;

namespace Opah.BancoCarrefour.Domain.Entidades;

public class SaldoDiarioEntity : BaseEntidade
{
    #region propriedades

    public DateTime Data { get; set; }
    public decimal TotalCreditos { get; set; }
    public decimal TotalDebitos { get; set; }
    public decimal SaldoConsolidado { get; set; }

    #endregion
}
