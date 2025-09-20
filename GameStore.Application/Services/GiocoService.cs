using GameStore.Domain.Entities;
using GameStore.Mapping;
using Microsoft.Extensions.Logging;

namespace GameStore.Application.Services;

/// <summary>
/// Implementazione del servizio per la gestione dei giochi
/// </summary>
public class GiocoService : IGiocoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMappingService _mappingService;
    private readonly ILogger<GiocoService> _logger;

    public GiocoService(IUnitOfWork unitOfWork, IMappingService mappingService, ILogger<GiocoService> logger)
    {
        _unitOfWork = unitOfWork;
        _mappingService = mappingService;
        _logger = logger;
    }

    public async Task<Result<GiocoDto>> GetByIdAsync(Guid id, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        Gioco? gioco = await _unitOfWork.Giochi.GetByIdAsync(id, includeDeleted, cancellationToken);
        if (gioco == null)
        {
            _logger.LogWarning("Gioco con ID {GiocoId} non trovato.", id);
            return Result<GiocoDto>.Failure(Errors.Giochi.NotFound);
        }
        return _mappingService.Map<Gioco, GiocoDto>(gioco);
    }

    public async Task<Result<PagedResult<GiocoDto>>> GetPagedAsync(FilterRequest request, CancellationToken cancellationToken = default)
    {
        PagedResult<Gioco> pagedResult = await _unitOfWork.Giochi.GetPagedAsync(request, cancellationToken: cancellationToken);
        IEnumerable<GiocoDto> dtoList = _mappingService.Map<Gioco, GiocoDto>(pagedResult.Items);

        return new PagedResult<GiocoDto>
        {
            Items = dtoList,
            TotalItems = pagedResult.TotalItems,
            PageNumber = pagedResult.PageNumber,
            PageSize = pagedResult.PageSize,
            TotalPages = pagedResult.TotalPages
        };
    }

    public async Task<Result<GiocoDto>> CreateAsync(CreaGiocoDto dto, CancellationToken cancellationToken = default)
    {
        Gioco gioco = _mappingService.Map<CreaGiocoDto, Gioco>(dto);
        await _unitOfWork.Giochi.AddAsync(gioco, cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Gioco {Titolo} creato con ID {GiocoId}.", gioco.Titolo, gioco.Id);
        return _mappingService.Map<Gioco, GiocoDto>(gioco);
    }

    public async Task<Result<GiocoDto>> UpdateAsync(AggiornaGiocoDto dto, CancellationToken cancellationToken = default)
    {
        Gioco? gioco = await _unitOfWork.Giochi.GetByIdAsync(dto.Id, false, cancellationToken);
        if (gioco == null)
        {
            _logger.LogWarning("Aggiornamento fallito: Gioco con ID {GiocoId} non trovato.", dto.Id);
            return Result<GiocoDto>.Failure(Errors.Giochi.NotFound);
        }

        _mappingService.Map(dto, gioco);
        _unitOfWork.Giochi.Update(gioco);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Gioco con ID {GiocoId} aggiornato.", gioco.Id);
        return _mappingService.Map<Gioco, GiocoDto>(gioco);
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Gioco? gioco = await _unitOfWork.Giochi.GetByIdAsync(id, false, cancellationToken);
        if (gioco == null)
        {
            _logger.LogWarning("Cancellazione fallita: Gioco con ID {GiocoId} non trovato.", id);
            return Result.Failure(Errors.Giochi.NotFound);
        }

        _unitOfWork.Giochi.Remove(gioco);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Gioco con ID {GiocoId} cancellato (soft delete).", id);
        return Result.Success();
    }

    public async Task<Result<bool>> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        bool exists = await _unitOfWork.Giochi.ExistsAsync(id, false, cancellationToken);
        return exists;
    }
}
