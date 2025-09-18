using GameStore.Application.Common;
using GameStore.Application.DTOs;
using GameStore.Domain.DTOs.Common;

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
    /// Ottiene i giochi più venduti
    /// </summary>
    /// <param name="count">Numero di giochi da restituire</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Risultato con lista dei giochi più venduti</returns>
    Task<Result<IEnumerable<GiocoDto>>> GetTopSellingAsync(int count = 10, CancellationToken cancellationToken = default);
}
