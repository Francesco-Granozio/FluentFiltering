using GameStore.Domain.Entities;
using GameStore.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameStore.Application.Services;

/// <summary>
/// Implementazione del servizio per la gestione delle recensioni
/// </summary>
public class RecensioneService : BaseService<Domain.Entities.Recensione, RecensioneDto>, IRecensioneService
{
    public RecensioneService(IUnitOfWork unitOfWork, IMappingService mappingService, ILogger<RecensioneService> logger)
        : base(unitOfWork, mappingService, logger)
    {
    }

    public async Task<Result<RecensioneDto>> GetByIdAsync(Guid id, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        Recensione? recensione = await UnitOfWork.Recensioni.GetByIdAsync(id, includeDeleted, cancellationToken);
        if (recensione == null)
        {
            Logger.LogWarning("Recensione con ID {RecensioneId} non trovata.", id);
            return Result<RecensioneDto>.Failure(Errors.Recensioni.NotFound);
        }
        return MappingService.Map<Recensione, RecensioneDto>(recensione);
    }

    public async Task<Result<PagedResult<RecensioneDto>>> GetPagedAsync(FilterRequest request, CancellationToken cancellationToken = default)
    {
        // Converte i nomi delle proprietà del DTO in nomi delle proprietà dell'entità
        FilterRequest modifiedRequest = ConvertDtoFilterToEntityFilter(request);

        PagedResult<Recensione> pagedResult = await UnitOfWork.Recensioni.GetPagedAsync(modifiedRequest,
            presetFilter: r => !r.IsCancellato,
            includes: q => q.Include(r => r.Utente).Include(r => r.Gioco),
            cancellationToken: cancellationToken);

        var dtoResult = MapPagedResult(pagedResult);

        return Result<PagedResult<RecensioneDto>>.Success(dtoResult);
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

    public async Task<Result<RecensioneDto>> CreateAsync(CreaRecensioneDto dto, CancellationToken cancellationToken = default)
    {
        // Verifica che l'utente esista
        Utente? utente = await UnitOfWork.Utenti.GetByIdAsync(dto.UtenteId, false, cancellationToken);
        if (utente == null)
        {
            return Result<RecensioneDto>.Failure(Errors.Recensioni.UserNotFound);
        }

        // Verifica che il gioco esista
        Gioco? gioco = await UnitOfWork.Giochi.GetByIdAsync(dto.GiocoId, false, cancellationToken);
        if (gioco == null)
        {
            return Result<RecensioneDto>.Failure(Errors.Recensioni.GameNotFound);
        }

        // Verifica che non esista già una recensione per questo utente e gioco
        Recensione? existingRecensione = await UnitOfWork.Recensioni.GetByUtenteEGiocoAsync(dto.UtenteId, dto.GiocoId, false, cancellationToken);
        if (existingRecensione != null)
        {
            return Result<RecensioneDto>.Failure(Errors.Recensioni.DuplicateReview);
        }

        // Verifica che l'acquisto esista se fornito
        if (dto.AcquistoId.HasValue)
        {
            Acquisto? acquisto = await UnitOfWork.Acquisti.GetByIdAsync(dto.AcquistoId.Value, false, cancellationToken);
            if (acquisto == null)
            {
                return Result<RecensioneDto>.Failure(Errors.Recensioni.InvalidPurchase);
            }
        }

        Recensione recensione = MappingService.Map<CreaRecensioneDto, Recensione>(dto);
        await UnitOfWork.Recensioni.AddAsync(recensione, cancellationToken);
        await UnitOfWork.SaveChangesAsync();

        Logger.LogInformation("Recensione creata con ID {RecensioneId} per utente {UtenteId} e gioco {GiocoId}.",
            recensione.Id, recensione.UtenteId, recensione.GiocoId);
        return MappingService.Map<Recensione, RecensioneDto>(recensione);
    }

    public async Task<Result<RecensioneDto>> UpdateAsync(AggiornaRecensioneDto dto, CancellationToken cancellationToken = default)
    {
        Recensione? recensione = await UnitOfWork.Recensioni.GetByIdAsync(dto.Id, false, cancellationToken);
        if (recensione == null)
        {
            Logger.LogWarning("Aggiornamento fallito: Recensione con ID {RecensioneId} non trovata.", dto.Id);
            return Result<RecensioneDto>.Failure(Errors.Recensioni.NotFound);
        }

        // Verifica che l'utente esista se è stato cambiato
        if (recensione.UtenteId != dto.UtenteId)
        {
            Utente? utente = await UnitOfWork.Utenti.GetByIdAsync(dto.UtenteId, false, cancellationToken);
            if (utente == null)
            {
                return Result<RecensioneDto>.Failure(Errors.Recensioni.UserNotFound);
            }
        }

        // Verifica che il gioco esista se è stato cambiato
        if (recensione.GiocoId != dto.GiocoId)
        {
            Gioco? gioco = await UnitOfWork.Giochi.GetByIdAsync(dto.GiocoId, false, cancellationToken);
            if (gioco == null)
            {
                return Result<RecensioneDto>.Failure(Errors.Recensioni.GameNotFound);
            }
        }

        // Verifica che l'acquisto esista se fornito e cambiato
        if (dto.AcquistoId.HasValue && recensione.AcquistoId != dto.AcquistoId)
        {
            Acquisto? acquisto = await UnitOfWork.Acquisti.GetByIdAsync(dto.AcquistoId.Value, false, cancellationToken);
            if (acquisto == null)
            {
                return Result<RecensioneDto>.Failure(Errors.Recensioni.InvalidPurchase);
            }
        }

        MappingService.Map(dto, recensione);
        UnitOfWork.Recensioni.Update(recensione);
        await UnitOfWork.SaveChangesAsync();

        Logger.LogInformation("Recensione con ID {RecensioneId} aggiornata.", recensione.Id);
        return MappingService.Map<Recensione, RecensioneDto>(recensione);
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Recensione? recensione = await UnitOfWork.Recensioni.GetByIdAsync(id, false, cancellationToken);
        if (recensione == null)
        {
            Logger.LogWarning("Cancellazione fallita: Recensione con ID {RecensioneId} non trovata.", id);
            return Result.Failure(Errors.Recensioni.NotFound);
        }

        UnitOfWork.Recensioni.Remove(recensione);
        await UnitOfWork.SaveChangesAsync();

        Logger.LogInformation("Recensione con ID {RecensioneId} cancellata (soft delete).", id);
        return Result.Success();
    }

    public async Task<Result<bool>> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        bool exists = await UnitOfWork.Recensioni.ExistsAsync(r => r.Id == id, false, cancellationToken);
        return exists;
    }

    public async Task<Result<IEnumerable<RecensioneDto>>> GetByGiocoAsync(Guid giocoId, CancellationToken cancellationToken = default)
    {
        IEnumerable<Recensione> recensioni = await UnitOfWork.Recensioni.GetByGiocoIdAsync(giocoId, false, cancellationToken);
        IEnumerable<RecensioneDto> dtoList = MappingService.Map<IEnumerable<Recensione>, IEnumerable<RecensioneDto>>(recensioni);
        return Result<IEnumerable<RecensioneDto>>.Success(dtoList);
    }
}
