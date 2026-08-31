namespace Opah.BancoCarrefour.Domain.Ports.Repositorios;

public interface ICommand<T> where T : class
{
    #region métodos

    Task<T> CreateAsync(T p, CancellationToken cancellationToken = default);
    Task<T> UpdateAsync(T p, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(T p, CancellationToken cancellationToken = default);

    #endregion
}
