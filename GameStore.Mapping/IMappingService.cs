namespace GameStore.Mapping;

/// <summary>
/// Servizio astratto per il mapping tra entità e DTO
/// </summary>
public interface IMappingService
{
    /// <summary>
    /// Mappa un oggetto di tipo TSource in un oggetto di tipo TDestination
    /// </summary>
    /// <typeparam name="TSource">Tipo sorgente</typeparam>
    /// <typeparam name="TDestination">Tipo destinazione</typeparam>
    /// <param name="source">Oggetto sorgente</param>
    /// <returns>Oggetto destinazione mappato</returns>
    TDestination Map<TSource, TDestination>(TSource source);

    /// <summary>
    /// Mappa una collezione di oggetti di tipo TSource in una collezione di oggetti di tipo TDestination
    /// </summary>
    /// <typeparam name="TSource">Tipo sorgente</typeparam>
    /// <typeparam name="TDestination">Tipo destinazione</typeparam>
    /// <param name="source">Collezione sorgente</param>
    /// <returns>Collezione destinazione mappata</returns>
    IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source);

    /// <summary>
    /// Mappa un oggetto di tipo TSource in un oggetto di tipo TDestination esistente
    /// </summary>
    /// <typeparam name="TSource">Tipo sorgente</typeparam>
    /// <typeparam name="TDestination">Tipo destinazione</typeparam>
    /// <param name="source">Oggetto sorgente</param>
    /// <param name="destination">Oggetto destinazione esistente</param>
    /// <returns>Oggetto destinazione aggiornato</returns>
    TDestination Map<TSource, TDestination>(TSource source, TDestination destination);

    /// <summary>
    /// Mappa un oggetto di tipo TSource in un oggetto di tipo TDestination in modo asincrono
    /// </summary>
    /// <typeparam name="TSource">Tipo sorgente</typeparam>
    /// <typeparam name="TDestination">Tipo destinazione</typeparam>
    /// <param name="source">Oggetto sorgente</param>
    /// <returns>Task con oggetto destinazione mappato</returns>
    Task<TDestination> MapAsync<TSource, TDestination>(TSource source);

    /// <summary>
    /// Mappa una collezione di oggetti di tipo TSource in una collezione di oggetti di tipo TDestination in modo asincrono
    /// </summary>
    /// <typeparam name="TSource">Tipo sorgente</typeparam>
    /// <typeparam name="TDestination">Tipo destinazione</typeparam>
    /// <param name="source">Collezione sorgente</param>
    /// <returns>Task con collezione destinazione mappata</returns>
    Task<IEnumerable<TDestination>> MapAsync<TSource, TDestination>(IEnumerable<TSource> source);
}
