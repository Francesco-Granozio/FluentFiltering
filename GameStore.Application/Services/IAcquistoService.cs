namespace GameStore.Application.Services;

/// <summary>
/// Interfaccia del servizio per la gestione degli acquisti
/// </summary>
public interface IAcquistoService
{
    /// <summary>
    /// Ottiene un acquisto per ID
    /// </summary>
    /// <param name="id">ID dell'acquisto</param>
    /// <param name="includeDeleted">Indica se includere acquisti cancellati</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Risultato con acquisto trovato o errore</returns>
    Task<Result<AcquistoDto>> GetByIdAsync(Guid id, bool includeDeleted = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ottiene acquisti paginati con filtro
    /// </summary>
    /// <param name="request">Richiesta di filtro e paginazione</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Risultato con lista paginata</returns>
    Task<Result<PagedResult<AcquistoDto>>> GetPagedAsync(FilterRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Crea un nuovo acquisto
    /// </summary>
    /// <param name="dto">DTO per la creazione</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Risultato con acquisto creato</returns>
    Task<Result<AcquistoDto>> CreateAsync(CreaAcquistoDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Aggiorna un acquisto esistente
    /// </summary>
    /// <param name="dto">DTO per l'aggiornamento</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Risultato con acquisto aggiornato</returns>
    Task<Result<AcquistoDto>> UpdateAsync(AggiornaAcquistoDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancella un acquisto (soft delete)
    /// </summary>
    /// <param name="id">ID dell'acquisto</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Risultato dell'operazione</returns>
    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica se un acquisto esiste
    /// </summary>
    /// <param name="id">ID dell'acquisto</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Risultato con valore booleano</returns>
    Task<Result<bool>> ExistsAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ottiene gli acquisti di un utente
    /// </summary>
    /// <param name="utenteId">ID dell'utente</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Lista degli acquisti dell'utente</returns>
    Task<Result<IEnumerable<AcquistoDto>>> GetByUtenteAsync(Guid utenteId, CancellationToken cancellationToken = default);
}