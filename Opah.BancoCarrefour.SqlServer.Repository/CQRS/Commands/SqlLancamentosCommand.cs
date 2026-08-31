using Opah.BancoCarrefour.Domain.Entidades;
using Opah.BancoCarrefour.Domain.Ports.Repositorios;

namespace Opah.BancoCarrefour.SqlServer.Repository.CQRS.Commands;

public class SqlLancamentosCommand : ICommand<LancamentosEntity>
{
    #region atributos

    private readonly SqlServerContext _context;

    #endregion

    #region constructors

    public SqlLancamentosCommand(SqlServerContext context)
        => _context = context;

    #endregion

    #region propriedades

    public async Task<LancamentosEntity> CreateAsync(LancamentosEntity p, CancellationToken cancellationToken = default)
    {
        await _context.Lancamentos.AddAsync(p, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return p;
    }

    public async Task<LancamentosEntity> UpdateAsync(LancamentosEntity p, CancellationToken cancellationToken = default)
    {
        _context.Lancamentos.Update(p).Property(e => e.DataCriacao).IsModified = false;
        await _context.SaveChangesAsync(cancellationToken);
        return p;
    }

    public async Task<bool> DeleteAsync(LancamentosEntity p, CancellationToken cancellationToken = default)
    {
        _context.Lancamentos.Remove(p);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    #endregion
}
