using GameStore.Domain.Entities;

namespace GameStore.Domain.Interfaces;

/// <summary>
/// Interfaccia repository specifica per Recensioni
/// </summary>
public interface IRecensioneRepository : IRepositoryGenerico<Recensione>
{
    /// <summary>
    /// Ottiene le recensioni di un utente
    /// </summary>
    /// <param name="utenteId">ID dell'utente</param>
    /// <param name="includeDeleted">Indica se includere recensioni cancellate</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Lista delle recensioni</returns>
    Task<IEnumerable<Recensione>> GetByUtenteIdAsync(Guid utenteId, bool includeDeleted = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ottiene le recensioni di un gioco
    /// </summary>
    /// <param name="giocoId">ID del gioco</param>
    /// <param name="includeDeleted">Indica se includere recensioni cancellate</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Lista delle recensioni</returns>
    Task<IEnumerable<Recensione>> GetByGiocoIdAsync(Guid giocoId, bool includeDeleted = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ottiene la recensione di un utente per un gioco specifico
    /// </summary>
    /// <param name="utenteId">ID dell'utente</param>
    /// <param name="giocoId">ID del gioco</param>
    /// <param name="includeDeleted">Indica se includere recensioni cancellate</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Recensione trovata o null</returns>
    Task<Recensione?> GetByUtenteEGiocoAsync(Guid utenteId, Guid giocoId, bool includeDeleted = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ottiene le recensioni verificate
    /// </summary>
    /// <param name="includeDeleted">Indica se includere recensioni cancellate</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Lista delle recensioni verificate</returns>
    Task<IEnumerable<Recensione>> GetRecensioniVerificateAsync(bool includeDeleted = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Calcola la media dei punteggi per un gioco
    /// </summary>
    /// <param name="giocoId">ID del gioco</param>
    /// <param name="includeDeleted">Indica se includere recensioni cancellate</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Media dei punteggi</returns>
    Task<double?> GetMediaPunteggiAsync(Guid giocoId, bool includeDeleted = false, CancellationToken cancellationToken = default);
}
