using Microsoft.EntityFrameworkCore;
using Opah.BancoCarrefour.Domain.Entidades;
using Opah.BancoCarrefour.Domain.Ports.Repositorios;

namespace Opah.BancoCarrefour.SqlServer.Repository.CQRS.Commands;

public class SqlSaldoDiarioConsolidacaoRepository : ISaldoDiarioConsolidacaoRepository
{
    #region atributos

    private readonly SqlServerContext _context;

    #endregion

    #region constructors

    public SqlSaldoDiarioConsolidacaoRepository(SqlServerContext context)
        => _context = context;

    #endregion

    #region métodos

    public async Task<bool> EventoJaProcessadoAsync(Guid eventoId, string consumerName, CancellationToken cancellationToken = default)
        => await _context.ProcessedEvents
            .AnyAsync(e => e.EventoId == eventoId && e.ConsumerName == consumerName, cancellationToken);

    public async Task ConsolidarAsync(
        DateTime data,
        decimal deltaCreditos,
        decimal deltaDebitos,
        Guid eventoId,
        string consumerName,
        CancellationToken cancellationToken = default)
    {
        var dataDoDia = data.Date;
        var saldo = await _context.SaldoDiario.FirstOrDefaultAsync(s => s.Data == dataDoDia, cancellationToken);

        if (saldo is null)
        {
            saldo = new SaldoDiarioEntity
            {
                Data = dataDoDia,
                TotalCreditos = deltaCreditos,
                TotalDebitos = deltaDebitos,
                SaldoConsolidado = deltaCreditos - deltaDebitos
            };
            await _context.SaldoDiario.AddAsync(saldo, cancellationToken);
        }
        else
        {
            saldo.TotalCreditos += deltaCreditos;
            saldo.TotalDebitos += deltaDebitos;
            saldo.SaldoConsolidado = saldo.TotalCreditos - saldo.TotalDebitos;
            saldo.DataAtualizacao = DateTime.UtcNow;
        }

        await _context.ProcessedEvents.AddAsync(new ProcessedEventEntity
        {
            EventoId = eventoId,
            ConsumerName = consumerName
        }, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    #endregion
}
