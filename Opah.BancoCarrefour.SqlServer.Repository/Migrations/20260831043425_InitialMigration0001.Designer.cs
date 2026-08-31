
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Opah.BancoCarrefour.SqlServer.Repository;

#nullable disable

namespace Opah.BancoCarrefour.SqlServer.Repository.Migrations
{
    [DbContext(typeof(SqlServerContext))]
    [Migration("20260831043425_InitialMigration0001")]
    partial class InitialMigration0001
    {

        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Opah.BancoCarrefour.Domain.Entidades.LancamentosEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DataAtualizacao")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataLancamento")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Tipo")
                        .HasColumnType("int");

                    b.Property<decimal>("Valor")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Lancamentos", (string)null);
                });

            modelBuilder.Entity("Opah.BancoCarrefour.Domain.Entidades.OutboxMessageEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DataAtualizacao")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("datetime2");

                    b.Property<string>("EventType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Payload")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ProcessedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("RoutingKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TentativasEnvio")
                        .HasColumnType("int");

                    b.Property<string>("UltimoErro")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ProcessedAt");

                    b.ToTable("OutboxMessages", (string)null);
                });

            modelBuilder.Entity("Opah.BancoCarrefour.Domain.Entidades.ProcessedEventEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConsumerName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime?>("DataAtualizacao")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("EventoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ProcessadoEm")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("EventoId", "ConsumerName")
                        .IsUnique();

                    b.ToTable("ProcessedEvents", (string)null);
                });

            modelBuilder.Entity("Opah.BancoCarrefour.Domain.Entidades.SaldoDiarioEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DataAtualizacao")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("SaldoConsolidado")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TotalCreditos")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TotalDebitos")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("Data")
                        .IsUnique();

                    b.ToTable("SaldoDiario", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
