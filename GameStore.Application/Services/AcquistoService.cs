using GameStore.Domain.Entities;
using GameStore.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameStore.Application.Services;

/// <summary>
/// Implementazione del servizio per la gestione degli acquisti
/// </summary>
public class AcquistoService : BaseService<Domain.Entities.Acquisto, AcquistoDto>, IAcquistoService
{
    public AcquistoService(IUnitOfWork unitOfWork, IMappingService mappingService, ILogger<AcquistoService> logger)
        : base(unitOfWork, mappingService, logger)
    {
    }

    public async Task<Result<AcquistoDto>> GetByIdAsync(Guid id, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        try
        {
            Acquisto? acquisto = await UnitOfWork.Acquisti.GetByIdAsync(id, includeDeleted, cancellationToken);
            if (acquisto == null)
            {
                Logger.LogWarning("Acquisto con ID {AcquistoId} non trovato.", id);
                return Result<AcquistoDto>.Failure(Errors.Acquisti.NotFound);
            }
            return MappingService.Map<Acquisto, AcquistoDto>(acquisto);
        }
        catch (Exception ex)
        {
            return HandleException<AcquistoDto>(ex, "il recupero", id);
        }
    }

    public async Task<Result<PagedResult<AcquistoDto>>> GetPagedAsync(FilterRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            // Converte i nomi delle proprietà del DTO in nomi delle proprietà dell'entità
            FilterRequest modifiedRequest = ConvertDtoFilterToEntityFilter(request);

            PagedResult<Acquisto> pagedResult = await UnitOfWork.Acquisti.GetPagedAsync(modifiedRequest,
                presetFilter: a => !a.IsCancellato,
                includes: q => q.Include(a => a.Utente).Include(a => a.Gioco),
                cancellationToken: cancellationToken);

            PagedResult<AcquistoDto> dtoResult = MapPagedResult(pagedResult);

            return Result<PagedResult<AcquistoDto>>.Success(dtoResult);
        }
        catch (Exception ex)
        {
            return HandleException<PagedResult<AcquistoDto>>(ex, "il recupero degli acquisti paginati");
        }
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
        Utente? utente = await UnitOfWork.Utenti.GetByIdAsync(dto.UtenteId, false, cancellationToken);
        if (utente == null)
        {
            return Result<AcquistoDto>.Failure(Errors.Acquisti.UserNotFound);
        }

        // Verifica che il gioco esista
        Gioco? gioco = await UnitOfWork.Giochi.GetByIdAsync(dto.GiocoId, false, cancellationToken);
        if (gioco == null)
        {
            return Result<AcquistoDto>.Failure(Errors.Acquisti.GameNotFound);
        }

        Acquisto acquisto = MappingService.Map<CreaAcquistoDto, Acquisto>(dto);
        await UnitOfWork.Acquisti.AddAsync(acquisto, cancellationToken);
        await UnitOfWork.SaveChangesAsync();

        Logger.LogInformation("Acquisto creato con ID {AcquistoId} per utente {UtenteId} e gioco {GiocoId}.",
            acquisto.Id, acquisto.UtenteId, acquisto.GiocoId);
        return MappingService.Map<Acquisto, AcquistoDto>(acquisto);
    }

    public async Task<Result<AcquistoDto>> UpdateAsync(AggiornaAcquistoDto dto, CancellationToken cancellationToken = default)
    {
        Acquisto? acquisto = await UnitOfWork.Acquisti.GetByIdAsync(dto.Id, false, cancellationToken);
        if (acquisto == null)
        {
            Logger.LogWarning("Aggiornamento fallito: Acquisto con ID {AcquistoId} non trovato.", dto.Id);
            return Result<AcquistoDto>.Failure(Errors.Acquisti.NotFound);
        }

        // Verifica che l'utente esista se è stato cambiato
        if (acquisto.UtenteId != dto.UtenteId)
        {
            Utente? utente = await UnitOfWork.Utenti.GetByIdAsync(dto.UtenteId, false, cancellationToken);
            if (utente == null)
            {
                return Result<AcquistoDto>.Failure(Errors.Acquisti.UserNotFound);
            }
        }

        // Verifica che il gioco esista se è stato cambiato
        if (acquisto.GiocoId != dto.GiocoId)
        {
            Gioco? gioco = await UnitOfWork.Giochi.GetByIdAsync(dto.GiocoId, false, cancellationToken);
            if (gioco == null)
            {
                return Result<AcquistoDto>.Failure(Errors.Acquisti.GameNotFound);
            }
        }

        MappingService.Map(dto, acquisto);
        UnitOfWork.Acquisti.Update(acquisto);
        await UnitOfWork.SaveChangesAsync();

        Logger.LogInformation("Acquisto con ID {AcquistoId} aggiornato.", acquisto.Id);
        return MappingService.Map<Acquisto, AcquistoDto>(acquisto);
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Acquisto? acquisto = await UnitOfWork.Acquisti.GetByIdAsync(id, false, cancellationToken);
        if (acquisto == null)
        {
            Logger.LogWarning("Cancellazione fallita: Acquisto con ID {AcquistoId} non trovato.", id);
            return Result.Failure(Errors.Acquisti.NotFound);
        }

        UnitOfWork.Acquisti.Remove(acquisto);
        await UnitOfWork.SaveChangesAsync();

        Logger.LogInformation("Acquisto con ID {AcquistoId} cancellato (soft delete).", id);
        return Result.Success();
    }

    public async Task<Result<bool>> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        bool exists = await UnitOfWork.Acquisti.ExistsAsync(id, false, cancellationToken);
        return exists;
    }

    public async Task<Result<IEnumerable<AcquistoDto>>> GetByUtenteAsync(Guid utenteId, CancellationToken cancellationToken = default)
    {
        IEnumerable<Acquisto> acquisti = await UnitOfWork.Acquisti.GetByUtenteIdAsync(utenteId, false, cancellationToken);
        IEnumerable<AcquistoDto> dtoList = MappingService.Map<IEnumerable<Acquisto>, IEnumerable<AcquistoDto>>(acquisti);
        return Result<IEnumerable<AcquistoDto>>.Success(dtoList);
    }
}
