namespace Opah.BancoCarrefour.Domain.Ports.Repositorios;

public interface ISaldoDiarioConsolidacaoRepository
{
    Task<bool> EventoJaProcessadoAsync(Guid eventoId, string consumerName, CancellationToken cancellationToken = default);

    Task ConsolidarAsync(
        DateTime data,
        decimal deltaCreditos,
        decimal deltaDebitos,
        Guid eventoId,
        string consumerName,
        CancellationToken cancellationToken = default);
}
