using System.Linq.Expressions;

namespace Opah.BancoCarrefour.Domain.Ports.Repositorios;

public interface IQuery<T> where T : class
{
	#region métodos

    Task<T?> GetByIdAsync(Guid? id, CancellationToken cancellationToken);

    Task<IEnumerable<T?>> GetAllAsync(CancellationToken cancellationToken);

    Task<IEnumerable<T?>> GetAllAsync(Expression<Func<T, bool>> filtro, CancellationToken cancellationToken);

    #endregion
}
