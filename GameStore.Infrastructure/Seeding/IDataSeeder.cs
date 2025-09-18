namespace GameStore.Infrastructure.Seeding;

/// <summary>
/// Interfaccia per il seeding dei dati iniziali del database
/// </summary>
public interface IDataSeeder
{
    /// <summary>
    /// Popola il database con dati di test
    /// </summary>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Task completato</returns>
    Task SeedAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica se il database è già popolato
    /// </summary>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>True se il database è già popolato, false altrimenti</returns>
    Task<bool> IsDatabaseSeededAsync(CancellationToken cancellationToken = default);
}
