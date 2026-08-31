using Microsoft.EntityFrameworkCore;
using Opah.BancoCarrefour.Domain.Entidades;
using Opah.BancoCarrefour.Domain.Ports.Repositorios;

namespace Opah.BancoCarrefour.SqlServer.Repository.CQRS.Commands;

public class SqlOutboxRepository : IOutboxRepository
{
    #region atributos

    private readonly SqlServerContext _context;

    #endregion

    #region constructors

    public SqlOutboxRepository(SqlServerContext context)
        => _context = context;

    #endregion

    #region métodos

    public async Task<IReadOnlyList<OutboxMessageEntity>> ObterPendentesAsync(int limite, CancellationToken cancellationToken = default)
        => await _context.OutboxMessages
            .Where(m => m.ProcessedAt == null)
            .OrderBy(m => m.DataCriacao)
            .Take(limite)
            .ToListAsync(cancellationToken);

    public async Task MarcarComoPublicadaAsync(Guid mensagemId, CancellationToken cancellationToken = default)
    {
        var mensagem = await _context.OutboxMessages.FirstOrDefaultAsync(m => m.Id == mensagemId, cancellationToken);
        if (mensagem is null)
            return;

        mensagem.ProcessedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RegistrarFalhaEnvioAsync(Guid mensagemId, string erro, CancellationToken cancellationToken = default)
    {
        var mensagem = await _context.OutboxMessages.FirstOrDefaultAsync(m => m.Id == mensagemId, cancellationToken);
        if (mensagem is null)
            return;

        mensagem.TentativasEnvio++;
        mensagem.UltimoErro = erro;
        await _context.SaveChangesAsync(cancellationToken);
    }

    #endregion
}
