using Opah.BancoCarrefour.Domain.Ports.Repositorios;
using Microsoft.EntityFrameworkCore;
using Opah.BancoCarrefour.Domain.Entidades;
using System.Linq.Expressions;

namespace Opah.BancoCarrefour.SqlServer.Repository.CQRS.Queries;

public class SqlSaldoDiarioQuery : IQuery<SaldoDiarioEntity>
{
    #region atributos

    private readonly SqlServerContext _context;

    #endregion

    #region constructors

    public SqlSaldoDiarioQuery(SqlServerContext context)
        => _context = context;

    #endregion

    #region propriedades

    public async Task<SaldoDiarioEntity?> GetByIdAsync(Guid? id, CancellationToken cancellationToken)
        => await _context.SaldoDiario
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    public async Task<IEnumerable<SaldoDiarioEntity?>> GetAllAsync(CancellationToken cancellationToken)
        => await _context.SaldoDiario
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<SaldoDiarioEntity?>> GetAllAsync(Expression<Func<SaldoDiarioEntity, bool>> filtro, CancellationToken cancellationToken)
        => await _context.SaldoDiario
            .Where(filtro)
            .ToListAsync(cancellationToken);

    #endregion
}
