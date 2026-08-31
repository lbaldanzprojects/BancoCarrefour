using Opah.BancoCarrefour.Domain.Entidades;

namespace Opah.BancoCarrefour.Domain.Ports.Repositorios;

public interface IOutboxRepository
{
    Task<IReadOnlyList<OutboxMessageEntity>> ObterPendentesAsync(int limite, CancellationToken cancellationToken = default);

    Task MarcarComoPublicadaAsync(Guid mensagemId, CancellationToken cancellationToken = default);

    Task RegistrarFalhaEnvioAsync(Guid mensagemId, string erro, CancellationToken cancellationToken = default);
}
