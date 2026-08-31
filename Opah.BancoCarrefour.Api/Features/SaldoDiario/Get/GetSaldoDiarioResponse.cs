namespace Opah.BancoCarrefour.Api.Features.SaldoDiario.Get;

public class GetSaldoDiarioResponse
{
    #region properties

    public Guid Id { get; set; }
    public DateTime Data { get; set; }
    public decimal TotalCreditos { get; set; }
    public decimal TotalDebitos { get; set; }
    public decimal SaldoConsolidado { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }

    #endregion
}
