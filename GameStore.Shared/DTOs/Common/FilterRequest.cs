namespace GameStore.Shared.DTOs.Common;

/// <summary>
/// Richiesta di filtro e paginazione per le query
/// </summary>
public class FilterRequest
{
    /// <summary>
    /// Filtro dinamico (es. "Titolo.Contains(\"war\") AND PrezzoListino < 20")
    /// </summary>
    public string? Filter { get; set; }

    /// <summary>
    /// Ordinamento dinamico (es. "PrezzoListino desc, Titolo asc")
    /// </summary>
    public string? OrderBy { get; set; }

    /// <summary>
    /// Numero della pagina (1-based, default: 1)
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Dimensione della pagina (default: 20)
    /// </summary>
    public int PageSize { get; set; } = 20;

    /// <summary>
    /// Indica se includere elementi cancellati (soft-delete)
    /// </summary>
    public bool IncludeDeleted { get; set; } = false;
}
