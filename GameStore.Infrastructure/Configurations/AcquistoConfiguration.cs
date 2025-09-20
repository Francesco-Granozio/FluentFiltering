using GameStore.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.Infrastructure.Configurations;

/// <summary>
/// Configurazione EF Core per l'entità Acquisto
/// </summary>
public class AcquistoConfiguration : IEntityTypeConfiguration<Acquisto>
{
    public void Configure(EntityTypeBuilder<Acquisto> builder)
    {
        // Nome tabella
        builder.ToTable("Acquisti");

        // Chiave primaria
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
            .HasDefaultValueSql("newsequentialid()");

        // Configurazione proprietà
        builder.Property(a => a.UtenteId)
            .IsRequired();

        builder.Property(a => a.GiocoId)
            .IsRequired();

        builder.Property(a => a.DataAcquisto)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.Property(a => a.PrezzoPagato)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(a => a.Quantita)
            .IsRequired()
            .HasDefaultValue(1);

        builder.Property(a => a.MetodoPagamento)
            .HasMaxLength(50);

        builder.Property(a => a.CodiceSconto)
            .HasMaxLength(50);

        // Configurazione auditing
        builder.Property(a => a.DataCreazione)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.Property(a => a.DataUltimaModifica)
            .HasColumnType("datetime2");

        // Configurazione soft delete
        builder.Property(a => a.IsCancellato)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(a => a.DataCancellazione)
            .HasColumnType("datetime2");

        // Indici per query temporali e report
        builder.HasIndex(a => a.DataAcquisto)
            .HasDatabaseName("IX_Acquisti_DataAcquisto");

        builder.HasIndex(a => a.UtenteId)
            .HasDatabaseName("IX_Acquisti_UtenteId");

        builder.HasIndex(a => a.GiocoId)
            .HasDatabaseName("IX_Acquisti_GiocoId");

        builder.HasIndex(a => new { a.UtenteId, a.GiocoId })
            .HasDatabaseName("IX_Acquisti_UtenteId_GiocoId");

        // Relazioni
        builder.HasOne(a => a.Utente)
            .WithMany(u => u.Acquisti)
            .HasForeignKey(a => a.UtenteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Gioco)
            .WithMany(g => g.Acquisti)
            .HasForeignKey(a => a.GiocoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(a => a.Recensioni)
            .WithOne(r => r.Acquisto)
            .HasForeignKey(r => r.AcquistoId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
