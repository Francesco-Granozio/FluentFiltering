using GameStore.Shared.DTOs;
using GameStore.Shared.DTOs.Common;

namespace GameStore.Domain.Interfaces;

/// <summary>
/// Interfaccia per il repository dei giochi acquistati
/// </summary>
public interface IGiochiAcquistatiRepository
{
    /// <summary>
    /// Ottiene i giochi acquistati con informazioni dell'utente e del gioco
    /// </summary>
    /// <param name="request">Richiesta di filtro e paginazione</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Risultato paginato dei giochi acquistati</returns>
    Task<PagedResult<GiochiAcquistatiDto>> GetGiochiAcquistatiAsync(
        FilterRequest request, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Ottiene tutti i giochi acquistati
    /// </summary>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Lista dei giochi acquistati</returns>
    Task<IEnumerable<GiochiAcquistatiDto>> GetAllGiochiAcquistatiAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Ottiene i giochi acquistati con filtro personalizzato
    /// </summary>
    /// <param name="filter">Filtro di ricerca (opzionale)</param>
    /// <param name="orderBy">Ordinamento (opzionale)</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Lista dei giochi acquistati filtrati</returns>
    Task<IEnumerable<GiochiAcquistatiDto>> GetGiochiAcquistatiAsync(
        string? filter = null,
        string? orderBy = null,
        CancellationToken cancellationToken = default);
}
