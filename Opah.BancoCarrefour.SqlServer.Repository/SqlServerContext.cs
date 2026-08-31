using Microsoft.EntityFrameworkCore;
using System.Data;
using Opah.BancoCarrefour.Domain.Entidades;

namespace Opah.BancoCarrefour.SqlServer.Repository;

public class SqlServerContext : DbContext
{
    #region atributos

    private readonly IDbConnection _dbConnection;

    #endregion

    #region construtores

    public SqlServerContext(DbContextOptions<SqlServerContext> options) : base(options)
        => _dbConnection = base.Database.GetDbConnection();

    #endregion

    #region métodos

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<LancamentosEntity>(p =>
        {
            p.ToTable("Lancamentos");
            p.HasKey(e => e.Id);

            p.Property(e => e.Id)
                .IsRequired();

            p.Property(e => e.Valor)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            p.Property(e => e.Tipo)
                .HasConversion<int>()
                .IsRequired();

            p.Property(e => e.Descricao)
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            p.Property(e => e.DataLancamento)
                .HasColumnType("datetime2")
                .IsRequired();

            p.Property(e => e.DataCriacao)
                .HasColumnType("datetime2")
                .IsRequired();

            p.Property(e => e.DataAtualizacao)
                .HasColumnType("datetime2");
        });

        builder.Entity<SaldoDiarioEntity>(p =>
        {
            p.ToTable("SaldoDiario");
            p.HasKey(e => e.Id);
            p.HasIndex(e => e.Data).IsUnique();

            p.Property(e => e.Id)
                .IsRequired();

            p.Property(e => e.Data)
                .HasColumnType("datetime2")
                .IsRequired();

            p.Property(e => e.TotalCreditos)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            p.Property(e => e.TotalDebitos)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            p.Property(e => e.SaldoConsolidado)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            p.Property(e => e.DataCriacao)
                .HasColumnType("datetime2")
                .IsRequired();

            p.Property(e => e.DataAtualizacao)
                .HasColumnType("datetime2");
        });

        builder.Entity<OutboxMessageEntity>(p =>
        {
            p.ToTable("OutboxMessages");
            p.HasKey(e => e.Id);
            p.HasIndex(e => e.ProcessedAt);

            p.Property(e => e.Id)
                .IsRequired();

            p.Property(e => e.EventType)
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            p.Property(e => e.RoutingKey)
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            p.Property(e => e.Payload)
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            p.Property(e => e.ProcessedAt)
                .HasColumnType("datetime2");

            p.Property(e => e.TentativasEnvio)
                .IsRequired();

            p.Property(e => e.UltimoErro)
                .HasColumnType("nvarchar(max)");

            p.Property(e => e.DataCriacao)
                .HasColumnType("datetime2")
                .IsRequired();

            p.Property(e => e.DataAtualizacao)
                .HasColumnType("datetime2");
        });

        builder.Entity<ProcessedEventEntity>(p =>
        {
            p.ToTable("ProcessedEvents");
            p.HasKey(e => e.Id);
            p.HasIndex(e => new { e.EventoId, e.ConsumerName }).IsUnique();

            p.Property(e => e.Id)
                .IsRequired();

            p.Property(e => e.EventoId)
                .IsRequired();

            p.Property(e => e.ConsumerName)
                .HasMaxLength(200)
                .IsRequired();

            p.Property(e => e.ProcessadoEm)
                .HasColumnType("datetime2")
                .IsRequired();

            p.Property(e => e.DataCriacao)
                .HasColumnType("datetime2")
                .IsRequired();

            p.Property(e => e.DataAtualizacao)
                .HasColumnType("datetime2");
        });
    }

    #endregion

    #region tabelas

    public DbSet<LancamentosEntity> Lancamentos { get; set; }

    public DbSet<SaldoDiarioEntity> SaldoDiario { get; set; }

    public DbSet<OutboxMessageEntity> OutboxMessages { get; set; }

    public DbSet<ProcessedEventEntity> ProcessedEvents { get; set; }

    #endregion

    #region propriedades

    public IDbConnection DbConnection => _dbConnection;

    #endregion
}
