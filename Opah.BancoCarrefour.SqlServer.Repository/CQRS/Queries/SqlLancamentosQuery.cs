using Opah.BancoCarrefour.Domain.Ports.Repositorios;
using Microsoft.EntityFrameworkCore;
using Opah.BancoCarrefour.Domain.Entidades;

namespace Opah.BancoCarrefour.SqlServer.Repository.CQRS.Queries;

public class SqlLancamentosQuery : IQuery<LancamentosEntity>
{
    #region atributos

    private readonly SqlServerContext _context;

    #endregion

    #region constructors

    public SqlLancamentosQuery(SqlServerContext context)
        => _context = context;

    #endregion

    #region propriedades

    public async Task<LancamentosEntity?> GetByIdAsync(Guid? id, CancellationToken cancellationToken)
        => await _context.Lancamentos
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    public async Task<IEnumerable<LancamentosEntity?>> GetAllAsync(CancellationToken cancellationToken)
        => await _context.Lancamentos
            .ToListAsync(cancellationToken);

    #endregion
}
