using GameStore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace GameStore.Infrastructure.Interceptors;

/// <summary>
/// Interceptor per gestire automaticamente auditing e soft delete
/// Intercetta le operazioni di salvataggio per aggiornare automaticamente:
/// - DataCreazione e DataUltimaModifica per le entità IAuditable
/// - IsCancellato e DataCancellazione per le entità ISoftDelete
/// </summary>
public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateAuditableEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateAuditableEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void UpdateAuditableEntities(DbContext? context)
    {
        if (context == null) return;

        var utcNow = DateTime.UtcNow;

        foreach (var entry in context.ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    HandleAddedEntity(entry, utcNow);
                    break;
                case EntityState.Modified:
                    HandleModifiedEntity(entry, utcNow);
                    break;
                case EntityState.Deleted:
                    HandleDeletedEntity(entry, utcNow);
                    break;
            }
        }
    }

    private static void HandleAddedEntity(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry, DateTime utcNow)
    {
        // Gestione auditing per entità aggiunte
        if (entry.Entity is IAuditable auditableEntity)
        {
            auditableEntity.DataCreazione = utcNow;
            auditableEntity.DataUltimaModifica = null; // Non modificata al momento della creazione
        }

        // Gestione soft delete per entità aggiunte
        if (entry.Entity is ISoftDelete softDeleteEntity)
        {
            softDeleteEntity.IsCancellato = false;
            softDeleteEntity.DataCancellazione = null;
        }
    }

    private static void HandleModifiedEntity(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry, DateTime utcNow)
    {
        // Gestione auditing per entità modificate
        if (entry.Entity is IAuditable auditableEntity)
        {
            // Assicurati che DataCreazione non venga mai modificata
            entry.Property(nameof(IAuditable.DataCreazione)).IsModified = false;
            
            // Aggiorna DataUltimaModifica
            auditableEntity.DataUltimaModifica = utcNow;
        }

        // Gestione soft delete per entità modificate
        if (entry.Entity is ISoftDelete softDeleteEntity)
        {
            // Se l'entità è stata cancellata logicamente, aggiorna la data di cancellazione
            if (softDeleteEntity.IsCancellato && softDeleteEntity.DataCancellazione == null)
            {
                softDeleteEntity.DataCancellazione = utcNow;
            }
        }
    }

    private static void HandleDeletedEntity(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry, DateTime utcNow)
    {
        // Gestione soft delete: converte la cancellazione fisica in cancellazione logica
        if (entry.Entity is ISoftDelete softDeleteEntity)
        {
            // Cambia lo stato da Deleted a Modified
            entry.State = EntityState.Modified;

            // Imposta le proprietà di soft delete
            softDeleteEntity.IsCancellato = true;
            softDeleteEntity.DataCancellazione = utcNow;

            // Se l'entità implementa anche IAuditable, aggiorna DataUltimaModifica
            if (entry.Entity is IAuditable auditableEntity)
            {
                auditableEntity.DataUltimaModifica = utcNow;
            }
        }
    }
}
