using GameStore.Domain.Entities;

namespace GameStore.Domain.Interfaces;

/// <summary>
/// Interfaccia repository specifica per Giochi
/// </summary>
public interface IGiocoRepository : IRepositoryGenerico<Gioco>
{
    /// <summary>
    /// Ottiene i giochi più venduti
    /// </summary>
    /// <param name="count">Numero di giochi da restituire</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Lista dei giochi più venduti</returns>
    Task<IEnumerable<Gioco>> GetTopSellingAsync(int count = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ottiene giochi per genere
    /// </summary>
    /// <param name="genere">Genere del gioco</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Lista di giochi</returns>
    Task<IEnumerable<Gioco>> GetByGenereAsync(string genere, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ottiene giochi per piattaforma
    /// </summary>
    /// <param name="piattaforma">Piattaforma del gioco</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Lista di giochi</returns>
    Task<IEnumerable<Gioco>> GetByPiattaformaAsync(string piattaforma, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ottiene giochi per sviluppatore
    /// </summary>
    /// <param name="sviluppatore">Sviluppatore del gioco</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Lista di giochi</returns>
    Task<IEnumerable<Gioco>> GetBySviluppatoreAsync(string sviluppatore, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica se un gioco esiste per ID
    /// </summary>
    /// <param name="id">ID del gioco</param>
    /// <param name="includeDeleted">Indica se includere giochi cancellati</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>True se il gioco esiste</returns>
    Task<bool> ExistsAsync(Guid id, bool includeDeleted = false, CancellationToken cancellationToken = default);
}
