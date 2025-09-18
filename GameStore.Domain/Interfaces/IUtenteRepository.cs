using GameStore.Domain.Entities;

namespace GameStore.Domain.Interfaces;

/// <summary>
/// Interfaccia repository specifica per Utenti
/// </summary>
public interface IUtenteRepository : IRepositoryGenerico<Utente>
{
    /// <summary>
    /// Ottiene un utente per username
    /// </summary>
    /// <param name="username">Username dell'utente</param>
    /// <param name="includeDeleted">Indica se includere utenti cancellati</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Utente trovato o null</returns>
    Task<Utente?> GetByUsernameAsync(string username, bool includeDeleted = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ottiene un utente per email
    /// </summary>
    /// <param name="email">Email dell'utente</param>
    /// <param name="includeDeleted">Indica se includere utenti cancellati</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Utente trovato o null</returns>
    Task<Utente?> GetByEmailAsync(string email, bool includeDeleted = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica se uno username esiste
    /// </summary>
    /// <param name="username">Username da verificare</param>
    /// <param name="excludeId">ID da escludere dalla verifica (per aggiornamenti)</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>True se lo username esiste</returns>
    Task<bool> UsernameExistsAsync(string username, Guid? excludeId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica se un'email esiste
    /// </summary>
    /// <param name="email">Email da verificare</param>
    /// <param name="excludeId">ID da escludere dalla verifica (per aggiornamenti)</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>True se l'email esiste</returns>
    Task<bool> EmailExistsAsync(string email, Guid? excludeId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica se un utente esiste per ID
    /// </summary>
    /// <param name="id">ID dell'utente</param>
    /// <param name="includeDeleted">Indica se includere utenti cancellati</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>True se l'utente esiste</returns>
    Task<bool> ExistsAsync(Guid id, bool includeDeleted = false, CancellationToken cancellationToken = default);
}
