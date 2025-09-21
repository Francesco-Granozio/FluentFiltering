using GameStore.Mapping;
using Microsoft.Extensions.Logging;

namespace GameStore.Application.Services;

/// <summary>
/// Servizio base con metodi comuni per tutti i servizi
/// </summary>
/// <typeparam name="TEntity">Tipo dell'entità</typeparam>
/// <typeparam name="TDto">Tipo del DTO</typeparam>
public abstract class BaseService<TEntity, TDto>
    where TEntity : class
    where TDto : class
{
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly IMappingService MappingService;
    protected readonly ILogger Logger;

    protected BaseService(IUnitOfWork unitOfWork, IMappingService mappingService, ILogger logger)
    {
        UnitOfWork = unitOfWork;
        MappingService = mappingService;
        Logger = logger;
    }

    /// <summary>
    /// Mappa un PagedResult di entità in un PagedResult di DTO
    /// </summary>
    /// <param name="pagedResult">Risultato paginato delle entità</param>
    /// <returns>Risultato paginato dei DTO</returns>
    protected PagedResult<TDto> MapPagedResult(PagedResult<TEntity> pagedResult)
    {
        IEnumerable<TDto> dtoList = MappingService.Map<TEntity, TDto>(pagedResult.Items);

        return new PagedResult<TDto>
        {
            Items = dtoList,
            TotalItems = pagedResult.TotalItems,
            PageNumber = pagedResult.PageNumber,
            PageSize = pagedResult.PageSize,
            TotalPages = pagedResult.TotalPages
        };
    }

    /// <summary>
    /// Gestisce un'eccezione e restituisce un Result di errore
    /// </summary>
    /// <typeparam name="T">Tipo del risultato</typeparam>
    /// <param name="ex">Eccezione catturata</param>
    /// <param name="operation">Nome dell'operazione</param>
    /// <param name="entityId">ID dell'entità (opzionale)</param>
    /// <returns>Result con errore</returns>
    protected Result<T> HandleException<T>(Exception ex, string operation, object? entityId = null)
    {
        string entityTypeName = typeof(TEntity).Name;
        string idMessage = entityId != null ? $" con ID {entityId}" : "";

        Logger.LogError(ex, "Errore durante {Operation} {EntityType}{IdMessage}",
            operation, entityTypeName, idMessage);

        return Result<T>.Failure(ErrorType.UnexpectedError,
            $"Errore durante {operation.ToLower()} {entityTypeName.ToLower()}");
    }

    /// <summary>
    /// Gestisce un'eccezione e restituisce un Result di errore senza valore
    /// </summary>
    /// <param name="ex">Eccezione catturata</param>
    /// <param name="operation">Nome dell'operazione</param>
    /// <param name="entityId">ID dell'entità (opzionale)</param>
    /// <returns>Result con errore</returns>
    protected Result HandleException(Exception ex, string operation, object? entityId = null)
    {
        string entityTypeName = typeof(TEntity).Name;
        string idMessage = entityId != null ? $" con ID {entityId}" : "";

        Logger.LogError(ex, "Errore durante {Operation} {EntityType}{IdMessage}",
            operation, entityTypeName, idMessage);

        return Result.Failure(ErrorType.UnexpectedError,
            $"Errore durante {operation.ToLower()} {entityTypeName.ToLower()}");
    }

    /// <summary>
    /// Logga un'operazione completata con successo
    /// </summary>
    /// <param name="operation">Nome dell'operazione</param>
    /// <param name="entityId">ID dell'entità</param>
    protected void LogSuccess(string operation, object entityId)
    {
        string entityTypeName = typeof(TEntity).Name;
        Logger.LogInformation("{Operation} {EntityType} con ID {EntityId} completato con successo",
            operation, entityTypeName, entityId);
    }
}
