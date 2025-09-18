using GameStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.Infrastructure.Configurations;

/// <summary>
/// Configurazione EF Core per l'entità Gioco
/// </summary>
public class GiocoConfiguration : IEntityTypeConfiguration<Gioco>
{
    public void Configure(EntityTypeBuilder<Gioco> builder)
    {
        // Nome tabella
        builder.ToTable("Giochi");

        // Chiave primaria
        builder.HasKey(g => g.Id);
        builder.Property(g => g.Id)
            .HasDefaultValueSql("newsequentialid()");

        // Configurazione proprietà
        builder.Property(g => g.Titolo)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(g => g.Descrizione)
            .HasColumnType("nvarchar(max)");

        builder.Property(g => g.PrezzoListino)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(g => g.DataRilascio)
            .HasColumnType("datetime2");

        builder.Property(g => g.Genere)
            .HasMaxLength(100);

        builder.Property(g => g.Piattaforma)
            .HasMaxLength(50);

        builder.Property(g => g.Sviluppatore)
            .HasMaxLength(150);

        // Configurazione auditing
        builder.Property(g => g.DataCreazione)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.Property(g => g.DataUltimaModifica)
            .HasColumnType("datetime2");

        // Configurazione soft delete
        builder.Property(g => g.IsCancellato)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(g => g.DataCancellazione)
            .HasColumnType("datetime2");

        // Indici per ricerca e filtri frequenti
        builder.HasIndex(g => g.Titolo)
            .HasDatabaseName("IX_Giochi_Titolo");

        builder.HasIndex(g => g.Genere)
            .HasDatabaseName("IX_Giochi_Genere");

        builder.HasIndex(g => g.Piattaforma)
            .HasDatabaseName("IX_Giochi_Piattaforma");

        builder.HasIndex(g => g.DataRilascio)
            .HasDatabaseName("IX_Giochi_DataRilascio");

        builder.HasIndex(g => g.PrezzoListino)
            .HasDatabaseName("IX_Giochi_PrezzoListino");

        // Navigazioni
        builder.HasMany(g => g.Acquisti)
            .WithOne(a => a.Gioco)
            .HasForeignKey(a => a.GiocoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(g => g.Recensioni)
            .WithOne(r => r.Gioco)
            .HasForeignKey(r => r.GiocoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
