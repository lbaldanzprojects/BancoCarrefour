using Opah.BancoCarrefour.Domain.Enums;

namespace Opah.BancoCarrefour.Api.Features.Lancamentos.Create;

public class CreateLancamentosRequest
{
    #region propriedades

    public decimal Valor { get; set; }
    public TipoLancamento Tipo { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public DateTime DataLancamento { get; set; }

    #endregion
}
