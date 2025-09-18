namespace GameStore.Domain.DTOs.Common;

/// <summary>
/// Risultato paginato per le query
/// </summary>
/// <typeparam name="T">Tipo degli elementi</typeparam>
public class PagedResult<T>
{
    /// <summary>
    /// Elementi della pagina corrente
    /// </summary>
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();

    /// <summary>
    /// Numero totale di elementi
    /// </summary>
    public int TotalItems { get; set; }

    /// <summary>
    /// Dimensione della pagina
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Numero della pagina corrente (1-based)
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Numero totale di pagine
    /// </summary>
    public int TotalPages { get; set; }
}
