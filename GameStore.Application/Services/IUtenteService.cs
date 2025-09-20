namespace GameStore.Application.Services;

/// <summary>
/// Interfaccia del servizio per la gestione degli utenti
/// </summary>
public interface IUtenteService
{
    /// <summary>
    /// Ottiene un utente per ID
    /// </summary>
    /// <param name="id">ID dell'utente</param>
    /// <param name="includeDeleted">Indica se includere utenti cancellati</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Risultato con utente trovato o errore</returns>
    Task<Result<UtenteDto>> GetByIdAsync(Guid id, bool includeDeleted = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ottiene utenti paginati con filtro
    /// </summary>
    /// <param name="request">Richiesta di filtro e paginazione</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Risultato con lista paginata</returns>
    Task<Result<PagedResult<UtenteDto>>> GetPagedAsync(FilterRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Crea un nuovo utente
    /// </summary>
    /// <param name="dto">DTO per la creazione</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Risultato con utente creato</returns>
    Task<Result<UtenteDto>> CreateAsync(CreaUtenteDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Aggiorna un utente esistente
    /// </summary>
    /// <param name="dto">DTO per l'aggiornamento</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Risultato con utente aggiornato</returns>
    Task<Result<UtenteDto>> UpdateAsync(AggiornaUtenteDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancella un utente (soft delete)
    /// </summary>
    /// <param name="id">ID dell'utente</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Risultato dell'operazione</returns>
    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica se un utente esiste
    /// </summary>
    /// <param name="id">ID dell'utente</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Risultato con valore booleano</returns>
    Task<Result<bool>> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
