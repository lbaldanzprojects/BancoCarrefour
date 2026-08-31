using Opah.BancoCarrefour.Domain.Enums;

namespace Opah.BancoCarrefour.Application.Handlers.Lancamentos.Delete;

public class DeleteLancamentosResult
{
    #region propriedades

    public Guid Id { get; set; }
    public decimal Valor { get; set; }
    public TipoLancamento Tipo { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public DateTime DataLancamento { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }

    #endregion
}
