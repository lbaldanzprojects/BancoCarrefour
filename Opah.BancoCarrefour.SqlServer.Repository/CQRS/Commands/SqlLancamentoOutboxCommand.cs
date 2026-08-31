using Opah.BancoCarrefour.Domain.Entidades;
using Opah.BancoCarrefour.Domain.Ports.Repositorios;

namespace Opah.BancoCarrefour.SqlServer.Repository.CQRS.Commands;

public class SqlLancamentoOutboxCommand : ILancamentoOutboxCommand
{
    #region atributos

    private readonly SqlServerContext _context;

    #endregion

    #region constructors

    public SqlLancamentoOutboxCommand(SqlServerContext context)
        => _context = context;

    #endregion

    #region métodos

    public async Task<LancamentosEntity> CreateComEventoAsync(
        LancamentosEntity lancamento,
        OutboxMessageEntity mensagemOutbox,
        CancellationToken cancellationToken = default)
    {
        await _context.Lancamentos.AddAsync(lancamento, cancellationToken);
        await _context.OutboxMessages.AddAsync(mensagemOutbox, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return lancamento;
    }

    #endregion
}
