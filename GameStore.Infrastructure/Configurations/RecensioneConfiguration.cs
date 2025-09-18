using GameStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.Infrastructure.Configurations;

/// <summary>
/// Configurazione EF Core per l'entità Recensione
/// </summary>
public class RecensioneConfiguration : IEntityTypeConfiguration<Recensione>
{
    public void Configure(EntityTypeBuilder<Recensione> builder)
    {
        // Nome tabella
        builder.ToTable("Recensioni");

        // Chiave primaria
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
            .HasDefaultValueSql("newsequentialid()");

        // Configurazione proprietà
        builder.Property(r => r.UtenteId)
            .IsRequired();

        builder.Property(r => r.GiocoId)
            .IsRequired();

        builder.Property(r => r.Punteggio)
            .IsRequired();

        builder.Property(r => r.Titolo)
            .HasMaxLength(200);

        builder.Property(r => r.Corpo)
            .HasColumnType("nvarchar(max)");

        builder.Property(r => r.DataRecensione)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.Property(r => r.IsRecensioneVerificata)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(r => r.AcquistoId);

        // Configurazione auditing
        builder.Property(r => r.DataCreazione)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.Property(r => r.DataUltimaModifica)
            .HasColumnType("datetime2");

        // Configurazione soft delete
        builder.Property(r => r.IsCancellato)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(r => r.DataCancellazione)
            .HasColumnType("datetime2");

        // Vincoli di dominio
        builder.ToTable("Recensioni", t => t.HasCheckConstraint("CK_Recensioni_Punteggio", "Punteggio BETWEEN 1 AND 5"));

        // Indici per aggregazioni e filtri
        builder.HasIndex(r => r.Punteggio)
            .HasDatabaseName("IX_Recensioni_Punteggio");

        builder.HasIndex(r => r.UtenteId)
            .HasDatabaseName("IX_Recensioni_UtenteId");

        builder.HasIndex(r => r.GiocoId)
            .HasDatabaseName("IX_Recensioni_GiocoId");

        builder.HasIndex(r => r.DataRecensione)
            .HasDatabaseName("IX_Recensioni_DataRecensione");

        builder.HasIndex(r => r.IsRecensioneVerificata)
            .HasDatabaseName("IX_Recensioni_IsRecensioneVerificata");

        // Indice composto per evitare recensioni multiple dello stesso utente per lo stesso gioco
        builder.HasIndex(r => new { r.UtenteId, r.GiocoId })
            .IsUnique()
            .HasDatabaseName("IX_Recensioni_UtenteId_GiocoId_Unique");

        // Relazioni
        builder.HasOne(r => r.Utente)
            .WithMany(u => u.Recensioni)
            .HasForeignKey(r => r.UtenteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Gioco)
            .WithMany(g => g.Recensioni)
            .HasForeignKey(r => r.GiocoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Acquisto)
            .WithMany(a => a.Recensioni)
            .HasForeignKey(r => r.AcquistoId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
