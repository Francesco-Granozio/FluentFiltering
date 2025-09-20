using GameStore.Domain.Entities;
using GameStore.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameStore.Application.Services;

/// <summary>
/// Implementazione del servizio per la gestione degli acquisti
/// </summary>
public class AcquistoService : IAcquistoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMappingService _mappingService;
    private readonly ILogger<AcquistoService> _logger;

    public AcquistoService(IUnitOfWork unitOfWork, IMappingService mappingService, ILogger<AcquistoService> logger)
    {
        _unitOfWork = unitOfWork;
        _mappingService = mappingService;
        _logger = logger;
    }

    public async Task<Result<AcquistoDto>> GetByIdAsync(Guid id, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        Acquisto? acquisto = await _unitOfWork.Acquisti.GetByIdAsync(id, includeDeleted, cancellationToken);
        if (acquisto == null)
        {
            _logger.LogWarning("Acquisto con ID {AcquistoId} non trovato.", id);
            return Result<AcquistoDto>.Failure(Errors.Acquisti.NotFound);
        }
        return _mappingService.Map<Acquisto, AcquistoDto>(acquisto);
    }

    public async Task<Result<PagedResult<AcquistoDto>>> GetPagedAsync(FilterRequest request, CancellationToken cancellationToken = default)
    {
        // Converte i nomi delle proprietà del DTO in nomi delle proprietà dell'entità
        FilterRequest modifiedRequest = ConvertDtoFilterToEntityFilter(request);

        PagedResult<Acquisto> pagedResult = await _unitOfWork.Acquisti.GetPagedAsync(modifiedRequest,
            presetFilter: a => !a.IsCancellato,
            includes: q => q.Include(a => a.Utente).Include(a => a.Gioco),
            cancellationToken: cancellationToken);

        IEnumerable<AcquistoDto> dtoList = _mappingService.Map<Acquisto, AcquistoDto>(pagedResult.Items);

        return new PagedResult<AcquistoDto>
        {
            Items = dtoList,
            TotalItems = pagedResult.TotalItems,
            PageNumber = pagedResult.PageNumber,
            PageSize = pagedResult.PageSize,
            TotalPages = pagedResult.TotalPages
        };
    }

    /// <summary>
    /// Converte i filtri del DTO in filtri dell'entità
    /// </summary>
    private FilterRequest ConvertDtoFilterToEntityFilter(FilterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Filter))
            return request;

        // Sostituisce i nomi delle proprietà del DTO con i nomi delle proprietà dell'entità
        string entityFilter = request.Filter
            .Replace("UtenteUsername", "Utente.Username")
            .Replace("GiocoTitolo", "Gioco.Titolo");

        return new FilterRequest
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            OrderBy = request.OrderBy,
            Filter = entityFilter
        };
    }

    public async Task<Result<AcquistoDto>> CreateAsync(CreaAcquistoDto dto, CancellationToken cancellationToken = default)
    {
        // Verifica che l'utente esista
        Utente? utente = await _unitOfWork.Utenti.GetByIdAsync(dto.UtenteId, false, cancellationToken);
        if (utente == null)
        {
            return Result<AcquistoDto>.Failure(Errors.Acquisti.UserNotFound);
        }

        // Verifica che il gioco esista
        Gioco? gioco = await _unitOfWork.Giochi.GetByIdAsync(dto.GiocoId, false, cancellationToken);
        if (gioco == null)
        {
            return Result<AcquistoDto>.Failure(Errors.Acquisti.GameNotFound);
        }

        Acquisto acquisto = _mappingService.Map<CreaAcquistoDto, Acquisto>(dto);
        await _unitOfWork.Acquisti.AddAsync(acquisto, cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Acquisto creato con ID {AcquistoId} per utente {UtenteId} e gioco {GiocoId}.",
            acquisto.Id, acquisto.UtenteId, acquisto.GiocoId);
        return _mappingService.Map<Acquisto, AcquistoDto>(acquisto);
    }

    public async Task<Result<AcquistoDto>> UpdateAsync(AggiornaAcquistoDto dto, CancellationToken cancellationToken = default)
    {
        Acquisto? acquisto = await _unitOfWork.Acquisti.GetByIdAsync(dto.Id, false, cancellationToken);
        if (acquisto == null)
        {
            _logger.LogWarning("Aggiornamento fallito: Acquisto con ID {AcquistoId} non trovato.", dto.Id);
            return Result<AcquistoDto>.Failure(Errors.Acquisti.NotFound);
        }

        // Verifica che l'utente esista se è stato cambiato
        if (acquisto.UtenteId != dto.UtenteId)
        {
            Utente? utente = await _unitOfWork.Utenti.GetByIdAsync(dto.UtenteId, false, cancellationToken);
            if (utente == null)
            {
                return Result<AcquistoDto>.Failure(Errors.Acquisti.UserNotFound);
            }
        }

        // Verifica che il gioco esista se è stato cambiato
        if (acquisto.GiocoId != dto.GiocoId)
        {
            Gioco? gioco = await _unitOfWork.Giochi.GetByIdAsync(dto.GiocoId, false, cancellationToken);
            if (gioco == null)
            {
                return Result<AcquistoDto>.Failure(Errors.Acquisti.GameNotFound);
            }
        }

        _mappingService.Map(dto, acquisto);
        _unitOfWork.Acquisti.Update(acquisto);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Acquisto con ID {AcquistoId} aggiornato.", acquisto.Id);
        return _mappingService.Map<Acquisto, AcquistoDto>(acquisto);
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Acquisto? acquisto = await _unitOfWork.Acquisti.GetByIdAsync(id, false, cancellationToken);
        if (acquisto == null)
        {
            _logger.LogWarning("Cancellazione fallita: Acquisto con ID {AcquistoId} non trovato.", id);
            return Result.Failure(Errors.Acquisti.NotFound);
        }

        _unitOfWork.Acquisti.Remove(acquisto);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Acquisto con ID {AcquistoId} cancellato (soft delete).", id);
        return Result.Success();
    }

    public async Task<Result<bool>> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        bool exists = await _unitOfWork.Acquisti.ExistsAsync(id, false, cancellationToken);
        return exists;
    }

    public async Task<Result<IEnumerable<AcquistoDto>>> GetByUtenteAsync(Guid utenteId, CancellationToken cancellationToken = default)
    {
        IEnumerable<Acquisto> acquisti = await _unitOfWork.Acquisti.GetByUtenteIdAsync(utenteId, false, cancellationToken);
        IEnumerable<AcquistoDto> dtoList = _mappingService.Map<IEnumerable<Acquisto>, IEnumerable<AcquistoDto>>(acquisti);
        return Result<IEnumerable<AcquistoDto>>.Success(dtoList);
    }
}
