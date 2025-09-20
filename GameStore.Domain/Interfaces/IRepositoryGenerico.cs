using GameStore.Shared.DTOs.Common;
using System.Linq.Expressions;

namespace GameStore.Domain.Interfaces;

/// <summary>
/// Interfaccia generica per i repository con operazioni CRUD e filtraggio dinamico
/// </summary>
/// <typeparam name="T">Tipo dell'entità</typeparam>
public interface IRepositoryGenerico<T> where T : class
{
    /// <summary>
    /// Ottiene un'entità per ID
    /// </summary>
    /// <param name="id">ID dell'entità</param>
    /// <param name="includeDeleted">Indica se includere entità cancellate</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Entità trovata o null</returns>
    Task<T?> GetByIdAsync(Guid id, bool includeDeleted = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ottiene tutte le entità
    /// </summary>
    /// <param name="includeDeleted">Indica se includere entità cancellate</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Lista di entità</returns>
    Task<IEnumerable<T>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ottiene entità paginate con filtro dinamico
    /// </summary>
    /// <param name="request">Richiesta di filtro e paginazione</param>
    /// <param name="presetFilter">Filtro predefinito aggiuntivo</param>
    /// <param name="includes">Include per le relazioni</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Risultato paginato</returns>
    Task<PagedResult<T>> GetPagedAsync(
        FilterRequest request,
        Expression<Func<T, bool>>? presetFilter = null,
        Func<IQueryable<T>, IQueryable<T>>? includes = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Aggiunge una nuova entità
    /// </summary>
    /// <param name="entity">Entità da aggiungere</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    Task AddAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Aggiunge più entità
    /// </summary>
    /// <param name="entities">Entità da aggiungere</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Aggiorna un'entità esistente
    /// </summary>
    /// <param name="entity">Entità da aggiornare</param>
    void Update(T entity);

    /// <summary>
    /// Aggiorna più entità
    /// </summary>
    /// <param name="entities">Entità da aggiornare</param>
    void UpdateRange(IEnumerable<T> entities);

    /// <summary>
    /// Rimuove un'entità (soft delete)
    /// </summary>
    /// <param name="entity">Entità da rimuovere</param>
    void Remove(T entity);

    /// <summary>
    /// Rimuove più entità (soft delete)
    /// </summary>
    /// <param name="entities">Entità da rimuovere</param>
    void RemoveRange(IEnumerable<T> entities);

    /// <summary>
    /// Rimuove definitivamente un'entità (hard delete)
    /// </summary>
    /// <param name="entity">Entità da rimuovere</param>
    void HardRemove(T entity);

    /// <summary>
    /// Verifica se un'entità esiste
    /// </summary>
    /// <param name="predicate">Predicato per la ricerca</param>
    /// <param name="includeDeleted">Indica se includere entità cancellate</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>True se l'entità esiste</returns>
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, bool includeDeleted = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Conta le entità che soddisfano un predicato
    /// </summary>
    /// <param name="predicate">Predicato per il conteggio</param>
    /// <param name="includeDeleted">Indica se includere entità cancellate</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Numero di entità</returns>
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, bool includeDeleted = false, CancellationToken cancellationToken = default);
}
