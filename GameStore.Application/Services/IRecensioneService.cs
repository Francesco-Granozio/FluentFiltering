namespace GameStore.Application.Services;

/// <summary>
/// Interfaccia del servizio per la gestione delle recensioni
/// </summary>
public interface IRecensioneService
{
    /// <summary>
    /// Ottiene una recensione per ID
    /// </summary>
    /// <param name="id">ID della recensione</param>
    /// <param name="includeDeleted">Indica se includere recensioni cancellate</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Risultato con recensione trovata o errore</returns>
    Task<Result<RecensioneDto>> GetByIdAsync(Guid id, bool includeDeleted = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ottiene recensioni paginate con filtro
    /// </summary>
    /// <param name="request">Richiesta di filtro e paginazione</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Risultato con lista paginata</returns>
    Task<Result<PagedResult<RecensioneDto>>> GetPagedAsync(FilterRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Crea una nuova recensione
    /// </summary>
    /// <param name="dto">DTO per la creazione</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Risultato con recensione creata</returns>
    Task<Result<RecensioneDto>> CreateAsync(CreaRecensioneDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Aggiorna una recensione esistente
    /// </summary>
    /// <param name="dto">DTO per l'aggiornamento</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Risultato con recensione aggiornata</returns>
    Task<Result<RecensioneDto>> UpdateAsync(AggiornaRecensioneDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancella una recensione (soft delete)
    /// </summary>
    /// <param name="id">ID della recensione</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Risultato dell'operazione</returns>
    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica se una recensione esiste
    /// </summary>
    /// <param name="id">ID della recensione</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Risultato con valore booleano</returns>
    Task<Result<bool>> ExistsAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ottiene le recensioni di un gioco
    /// </summary>
    /// <param name="giocoId">ID del gioco</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Lista delle recensioni del gioco</returns>
    Task<Result<IEnumerable<RecensioneDto>>> GetByGiocoAsync(Guid giocoId, CancellationToken cancellationToken = default);
}