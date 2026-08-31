using MediatR;
using Opah.BancoCarrefour.Domain.Enums;

namespace Opah.BancoCarrefour.Application.Handlers.Lancamentos.Create;

public record CreateLancamentosCommand : IRequest<CreateLancamentosResult>
{
    #region propriedades

    public decimal Valor { get; set; }
    public TipoLancamento Tipo { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public DateTime DataLancamento { get; set; }

    #endregion
}
