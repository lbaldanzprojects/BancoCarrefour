namespace Opah.BancoCarrefour.Api.Features.SaldoDiario.Get;

public class GetLancamentosResponse
{
    #region properties

    public Guid Id { get; set; } = Guid.Empty;
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }

    #endregion
}
