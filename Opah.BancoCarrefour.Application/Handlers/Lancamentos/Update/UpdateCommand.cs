using MediatR;
using Opah.BancoCarrefour.Domain.Enums;

namespace Opah.BancoCarrefour.Application.Handlers.Lancamentos.Update;

public record UpdateLancamentosCommand : IRequest<UpdateLancamentosResult>
{
    #region propriedades

    public Guid Id { get; set; }
    public decimal Valor { get; set; }
    public TipoLancamento Tipo { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public DateTime DataLancamento { get; set; }

    #endregion
}
