using GameStore.Mapping;
using Microsoft.Extensions.Logging;

namespace GameStore.Application.Services;

/// <summary>
/// Implementazione del servizio per la gestione dei giochi acquistati
/// </summary>
public class GiochiAcquistatiService : IGiochiAcquistatiService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMappingService _mappingService;
    private readonly ILogger<GiochiAcquistatiService> _logger;

    public GiochiAcquistatiService(
        IUnitOfWork unitOfWork,
        IMappingService mappingService,
        ILogger<GiochiAcquistatiService> logger)
    {
        _unitOfWork = unitOfWork;
        _mappingService = mappingService;
        _logger = logger;
    }

    /// <summary>
    /// Ottiene i giochi acquistati paginati
    /// </summary>
    public async Task<Result<PagedResult<GiochiAcquistatiDto>>> GetPagedAsync(
        FilterRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Recupero giochi acquistati paginati. Pagina: {PageNumber}, Dimensione: {PageSize}",
                request.PageNumber, request.PageSize);

            PagedResult<GiochiAcquistatiDto> pagedResult = await _unitOfWork.GiochiAcquistati
                .GetGiochiAcquistatiAsync(request, cancellationToken);

            _logger.LogInformation("Recuperati {Count} giochi acquistati su {Total} totali",
                pagedResult.Items.Count(), pagedResult.TotalItems);

            return Result<PagedResult<GiochiAcquistatiDto>>.Success(pagedResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero dei giochi acquistati paginati");
            return Result<PagedResult<GiochiAcquistatiDto>>.Failure(ErrorType.UnexpectedError,
                "Errore durante il recupero dei giochi acquistati");
        }
    }

    /// <summary>
    /// Ottiene tutti i giochi acquistati
    /// </summary>
    public async Task<Result<IEnumerable<GiochiAcquistatiDto>>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Recupero di tutti i giochi acquistati");

            IEnumerable<GiochiAcquistatiDto> giochiAcquistati = await _unitOfWork.GiochiAcquistati
                .GetAllGiochiAcquistatiAsync(cancellationToken);

            _logger.LogInformation("Recuperati {Count} giochi acquistati", giochiAcquistati.Count());

            return Result<IEnumerable<GiochiAcquistatiDto>>.Success(giochiAcquistati);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero di tutti i giochi acquistati");
            return Result<IEnumerable<GiochiAcquistatiDto>>.Failure(ErrorType.UnexpectedError,
                "Errore durante il recupero dei giochi acquistati");
        }
    }

    /// <summary>
    /// Ottiene i giochi acquistati con filtro personalizzato
    /// </summary>
    public async Task<Result<IEnumerable<GiochiAcquistatiDto>>> GetFilteredAsync(
        string? filter = null,
        string? orderBy = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Recupero giochi acquistati con filtro: '{Filter}', ordinamento: '{OrderBy}'",
                filter ?? "nessuno", orderBy ?? "default");

            IEnumerable<GiochiAcquistatiDto> giochiAcquistati = await _unitOfWork.GiochiAcquistati
                .GetGiochiAcquistatiAsync(filter, orderBy, cancellationToken);

            _logger.LogInformation("Recuperati {Count} giochi acquistati filtrati", giochiAcquistati.Count());

            return Result<IEnumerable<GiochiAcquistatiDto>>.Success(giochiAcquistati);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero dei giochi acquistati filtrati");
            return Result<IEnumerable<GiochiAcquistatiDto>>.Failure(ErrorType.UnexpectedError,
                "Errore durante il recupero dei giochi acquistati filtrati");
        }
    }
}
