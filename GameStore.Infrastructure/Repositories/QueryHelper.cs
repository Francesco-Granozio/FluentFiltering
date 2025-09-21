namespace GameStore.Infrastructure.Repositories;

/// <summary>
/// Helper per costruire query comuni nei repository
/// </summary>
public static class QueryHelper
{
    /// <summary>
    /// Crea una query base con gestione del soft delete
    /// </summary>
    /// <typeparam name="T">Tipo dell'entità</typeparam>
    /// <param name="dbSet">DbSet dell'entità</param>
    /// <param name="includeDeleted">Indica se includere entità cancellate</param>
    /// <returns>Query base</returns>
    public static IQueryable<T> CreateBaseQuery<T>(DbSet<T> dbSet, bool includeDeleted = false) where T : class
    {
        IQueryable<T> query = dbSet.AsQueryable();

        if (includeDeleted)
        {
            query = query.IgnoreQueryFilters();
        }

        return query;
    }

    /// <summary>
    /// Applica un filtro di esclusione per ID
    /// </summary>
    /// <typeparam name="T">Tipo dell'entità</typeparam>
    /// <param name="query">Query base</param>
    /// <param name="excludeId">ID da escludere</param>
    /// <returns>Query con filtro di esclusione</returns>
    public static IQueryable<T> ExcludeById<T>(this IQueryable<T> query, Guid? excludeId) where T : class
    {
        if (!excludeId.HasValue)
            return query;

        return query.Where(x => EF.Property<Guid>(x, "Id") != excludeId.Value);
    }

    /// <summary>
    /// Applica un filtro per proprietà stringa
    /// </summary>
    /// <typeparam name="T">Tipo dell'entità</typeparam>
    /// <param name="query">Query base</param>
    /// <param name="propertyName">Nome della proprietà</param>
    /// <param name="value">Valore da cercare</param>
    /// <param name="exactMatch">Indica se fare match esatto o parziale</param>
    /// <returns>Query con filtro applicato</returns>
    public static IQueryable<T> FilterByProperty<T>(this IQueryable<T> query, string propertyName, string value, bool exactMatch = false) where T : class
    {
        if (string.IsNullOrEmpty(value))
            return query;

        return exactMatch
            ? query.Where(x => EF.Property<string>(x, propertyName) == value)
            : query.Where(x => EF.Property<string>(x, propertyName).Contains(value));
    }
}
