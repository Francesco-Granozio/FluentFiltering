namespace GameStore.Application.Services;

/// <summary>
/// Interfaccia del servizio per la gestione dei giochi
/// </summary>
public interface IGiocoService
{
    /// <summary>
    /// Ottiene un gioco per ID
    /// </summary>
    /// <param name="id">ID del gioco</param>
    /// <param name="includeDeleted">Indica se includere giochi cancellati</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Risultato con gioco trovato o errore</returns>
    Task<Result<GiocoDto>> GetByIdAsync(Guid id, bool includeDeleted = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ottiene giochi paginati con filtro
    /// </summary>
    /// <param name="request">Richiesta di filtro e paginazione</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Risultato con lista paginata</returns>
    Task<Result<PagedResult<GiocoDto>>> GetPagedAsync(FilterRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Crea un nuovo gioco
    /// </summary>
    /// <param name="dto">DTO per la creazione</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Risultato con gioco creato</returns>
    Task<Result<GiocoDto>> CreateAsync(CreaGiocoDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Aggiorna un gioco esistente
    /// </summary>
    /// <param name="dto">DTO per l'aggiornamento</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Risultato con gioco aggiornato</returns>
    Task<Result<GiocoDto>> UpdateAsync(AggiornaGiocoDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancella un gioco (soft delete)
    /// </summary>
    /// <param name="id">ID del gioco</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Risultato dell'operazione</returns>
    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica se un gioco esiste
    /// </summary>
    /// <param name="id">ID del gioco</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Risultato con valore booleano</returns>
    Task<Result<bool>> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}