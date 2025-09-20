namespace GameStore.Mapping;

/// <summary>
/// Implementazione del servizio di mapping utilizzando AutoMapper
/// </summary>
public class AutoMapperService : IMappingService
{
    private readonly IMapper _mapper;

    public AutoMapperService(IMapper mapper)
    {
        _mapper = mapper;
    }

    /// <summary>
    /// Mappa un oggetto di tipo TSource in un oggetto di tipo TDestination
    /// </summary>
    public TDestination Map<TSource, TDestination>(TSource source)
    {
        return _mapper.Map<TDestination>(source);
    }

    /// <summary>
    /// Mappa una collezione di oggetti di tipo TSource in una collezione di oggetti di tipo TDestination
    /// </summary>
    public IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source)
    {
        return _mapper.Map<IEnumerable<TDestination>>(source);
    }

    /// <summary>
    /// Mappa un oggetto di tipo TSource in un oggetto di tipo TDestination esistente
    /// </summary>
    public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
    {
        return _mapper.Map(source, destination);
    }

    /// <summary>
    /// Mappa un oggetto di tipo TSource in un oggetto di tipo TDestination in modo asincrono
    /// </summary>
    public async Task<TDestination> MapAsync<TSource, TDestination>(TSource source)
    {
        return await Task.FromResult(_mapper.Map<TDestination>(source));
    }

    /// <summary>
    /// Mappa una collezione di oggetti di tipo TSource in una collezione di oggetti di tipo TDestination in modo asincrono
    /// </summary>
    public async Task<IEnumerable<TDestination>> MapAsync<TSource, TDestination>(IEnumerable<TSource> source)
    {
        return await Task.FromResult(_mapper.Map<IEnumerable<TDestination>>(source));
    }
}
