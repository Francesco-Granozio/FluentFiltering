using GameStore.Domain.Entities;

namespace GameStore.Domain.Interfaces;

/// <summary>
/// Interfaccia repository specifica per Acquisti
/// </summary>
public interface IAcquistoRepository : IRepositoryGenerico<Acquisto>
{
    /// <summary>
    /// Ottiene gli acquisti di un utente
    /// </summary>
    /// <param name="utenteId">ID dell'utente</param>
    /// <param name="includeDeleted">Indica se includere acquisti cancellati</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Lista degli acquisti</returns>
    Task<IEnumerable<Acquisto>> GetByUtenteIdAsync(Guid utenteId, bool includeDeleted = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ottiene gli acquisti di un gioco
    /// </summary>
    /// <param name="giocoId">ID del gioco</param>
    /// <param name="includeDeleted">Indica se includere acquisti cancellati</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Lista degli acquisti</returns>
    Task<IEnumerable<Acquisto>> GetByGiocoIdAsync(Guid giocoId, bool includeDeleted = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica se un utente ha acquistato un gioco
    /// </summary>
    /// <param name="utenteId">ID dell'utente</param>
    /// <param name="giocoId">ID del gioco</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>True se l'acquisto esiste</returns>
    Task<bool> HasUtenteAcquistatoGiocoAsync(Guid utenteId, Guid giocoId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ottiene acquisti per periodo
    /// </summary>
    /// <param name="dataInizio">Data di inizio</param>
    /// <param name="dataFine">Data di fine</param>
    /// <param name="includeDeleted">Indica se includere acquisti cancellati</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Lista degli acquisti</returns>
    Task<IEnumerable<Acquisto>> GetByPeriodoAsync(DateTime dataInizio, DateTime dataFine, bool includeDeleted = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica se un acquisto esiste per ID
    /// </summary>
    /// <param name="id">ID dell'acquisto</param>
    /// <param name="includeDeleted">Indica se includere acquisti cancellati</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>True se l'acquisto esiste</returns>
    Task<bool> ExistsAsync(Guid id, bool includeDeleted = false, CancellationToken cancellationToken = default);
}
