namespace GameStore.Application.Services;

/// <summary>
/// Interfaccia del servizio per la gestione dei giochi acquistati
/// </summary>
public interface IGiochiAcquistatiService
{
    /// <summary>
    /// Ottiene i giochi acquistati paginati
    /// </summary>
    /// <param name="request">Richiesta di filtro e paginazione</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Risultato paginato dei giochi acquistati</returns>
    Task<Result<PagedResult<GiochiAcquistatiDto>>> GetPagedAsync(
        FilterRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Ottiene tutti i giochi acquistati
    /// </summary>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Lista dei giochi acquistati</returns>
    Task<Result<IEnumerable<GiochiAcquistatiDto>>> GetAllAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Ottiene i giochi acquistati con filtro personalizzato
    /// </summary>
    /// <param name="filter">Filtro di ricerca (opzionale)</param>
    /// <param name="orderBy">Ordinamento (opzionale)</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Lista dei giochi acquistati filtrati</returns>
    Task<Result<IEnumerable<GiochiAcquistatiDto>>> GetFilteredAsync(
        string? filter = null,
        string? orderBy = null,
        CancellationToken cancellationToken = default);
}
