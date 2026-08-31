using Opah.BancoCarrefour.Domain.Entidades;

namespace Opah.BancoCarrefour.Domain.Ports.Repositorios;

public interface ILancamentoOutboxCommand
{
    Task<LancamentosEntity> CreateComEventoAsync(
        LancamentosEntity lancamento,
        OutboxMessageEntity mensagemOutbox,
        CancellationToken cancellationToken = default);
}
