using Opah.BancoCarrefour.Domain.Entidades;
using Opah.BancoCarrefour.Domain.Ports.Repositorios;

namespace Opah.BancoCarrefour.SqlServer.Repository.CQRS.Commands;

public class SqlSaldoDiarioCommand : ICommand<SaldoDiarioEntity>
{
    #region atributos

    private readonly SqlServerContext _context;

    #endregion

    #region constructors

    public SqlSaldoDiarioCommand(SqlServerContext context)
        => _context = context;

    #endregion

    #region propriedades

    public async Task<SaldoDiarioEntity> CreateAsync(SaldoDiarioEntity p, CancellationToken cancellationToken = default)
    {
        await _context.SaldoDiario.AddAsync(p, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return p;
    }

    public async Task<SaldoDiarioEntity> UpdateAsync(SaldoDiarioEntity p, CancellationToken cancellationToken = default)
    {
        _context.SaldoDiario.Update(p).Property(e => e.DataCriacao).IsModified = false;
        await _context.SaveChangesAsync(cancellationToken);
        return p;
    }

    public async Task<bool> DeleteAsync(SaldoDiarioEntity p, CancellationToken cancellationToken = default)
    {
        _context.SaldoDiario.Remove(p);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    #endregion
}
