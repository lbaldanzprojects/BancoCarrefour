using Opah.BancoCarrefour.Domain.Common;
using Opah.BancoCarrefour.Domain.Enums;

namespace Opah.BancoCarrefour.Domain.Entidades;

public class LancamentosEntity : BaseEntidade
{
    #region proprieades

    public decimal Valor { get; set; }

    public TipoLancamento Tipo { get; set; }

    public string Descricao { get; set; } = string.Empty;

    public DateTime DataLancamento { get; set; }

    #endregion
}
