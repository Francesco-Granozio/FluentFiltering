using GameStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.Infrastructure.Configurations;

/// <summary>
/// Configurazione EF Core per l'entità Utente
/// </summary>
public class UtenteConfiguration : IEntityTypeConfiguration<Utente>
{
    public void Configure(EntityTypeBuilder<Utente> builder)
    {
        // Nome tabella
        builder.ToTable("Utenti");

        // Chiave primaria
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .HasDefaultValueSql("newsequentialid()");

        // Configurazione proprietà
        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(254);

        builder.Property(u => u.NomeCompleto)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(u => u.Paese)
            .HasMaxLength(100);

        builder.Property(u => u.DataRegistrazione)
            .IsRequired()
            .HasColumnType("datetime2");

        // Configurazione auditing
        builder.Property(u => u.DataCreazione)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.Property(u => u.DataUltimaModifica)
            .HasColumnType("datetime2");

        // Configurazione soft delete
        builder.Property(u => u.IsCancellato)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(u => u.DataCancellazione)
            .HasColumnType("datetime2");

        // Indici
        builder.HasIndex(u => u.Username)
            .IsUnique()
            .HasDatabaseName("IX_Utenti_Username_Unique");

        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("IX_Utenti_Email_Unique");

        builder.HasIndex(u => u.DataRegistrazione)
            .HasDatabaseName("IX_Utenti_DataRegistrazione");

        // Navigazioni
        builder.HasMany(u => u.Acquisti)
            .WithOne(a => a.Utente)
            .HasForeignKey(a => a.UtenteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.Recensioni)
            .WithOne(r => r.Utente)
            .HasForeignKey(r => r.UtenteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
