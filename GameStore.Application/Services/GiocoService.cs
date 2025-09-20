using GameStore.Domain.Entities;
using GameStore.Mapping;
using Microsoft.Extensions.Logging;

namespace GameStore.Application.Services;

/// <summary>
/// Implementazione del servizio per la gestione dei giochi
/// </summary>
public class GiocoService : BaseService<Domain.Entities.Gioco, GiocoDto>, IGiocoService
{
    public GiocoService(IUnitOfWork unitOfWork, IMappingService mappingService, ILogger<GiocoService> logger)
        : base(unitOfWork, mappingService, logger)
    {
    }

    public async Task<Result<GiocoDto>> GetByIdAsync(Guid id, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        try
        {
            Gioco? gioco = await UnitOfWork.Giochi.GetByIdAsync(id, includeDeleted, cancellationToken);
            if (gioco == null)
            {
                Logger.LogWarning("Gioco con ID {GiocoId} non trovato.", id);
                return Result<GiocoDto>.Failure(Errors.Giochi.NotFound);
            }
            return MappingService.Map<Gioco, GiocoDto>(gioco);
        }
        catch (Exception ex)
        {
            return HandleException<GiocoDto>(ex, "il recupero", id);
        }
    }

    public async Task<Result<PagedResult<GiocoDto>>> GetPagedAsync(FilterRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            PagedResult<Gioco> pagedResult = await UnitOfWork.Giochi.GetPagedAsync(request, cancellationToken: cancellationToken);
            var dtoResult = MapPagedResult(pagedResult);

            return Result<PagedResult<GiocoDto>>.Success(dtoResult);
        }
        catch (Exception ex)
        {
            return HandleException<PagedResult<GiocoDto>>(ex, "il recupero dei giochi paginati");
        }
    }

    public async Task<Result<GiocoDto>> CreateAsync(CreaGiocoDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            Gioco gioco = MappingService.Map<CreaGiocoDto, Gioco>(dto);
            await UnitOfWork.Giochi.AddAsync(gioco, cancellationToken);
            await UnitOfWork.SaveChangesAsync();

            LogSuccess("Creazione", gioco.Id);
            return MappingService.Map<Gioco, GiocoDto>(gioco);
        }
        catch (Exception ex)
        {
            return HandleException<GiocoDto>(ex, "la creazione");
        }
    }

    public async Task<Result<GiocoDto>> UpdateAsync(AggiornaGiocoDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            Gioco? gioco = await UnitOfWork.Giochi.GetByIdAsync(dto.Id, false, cancellationToken);
            if (gioco == null)
            {
                Logger.LogWarning("Aggiornamento fallito: Gioco con ID {GiocoId} non trovato.", dto.Id);
                return Result<GiocoDto>.Failure(Errors.Giochi.NotFound);
            }

            MappingService.Map(dto, gioco);
            UnitOfWork.Giochi.Update(gioco);
            await UnitOfWork.SaveChangesAsync();

            LogSuccess("Aggiornamento", gioco.Id);
            return MappingService.Map<Gioco, GiocoDto>(gioco);
        }
        catch (Exception ex)
        {
            return HandleException<GiocoDto>(ex, "l'aggiornamento", dto.Id);
        }
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            Gioco? gioco = await UnitOfWork.Giochi.GetByIdAsync(id, false, cancellationToken);
            if (gioco == null)
            {
                Logger.LogWarning("Cancellazione fallita: Gioco con ID {GiocoId} non trovato.", id);
                return Result.Failure(Errors.Giochi.NotFound);
            }

            UnitOfWork.Giochi.Remove(gioco);
            await UnitOfWork.SaveChangesAsync();

            LogSuccess("Cancellazione", id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return HandleException(ex, "la cancellazione", id);
        }
    }

    public async Task<Result<bool>> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            bool exists = await UnitOfWork.Giochi.ExistsAsync(id, false, cancellationToken);
            return Result<bool>.Success(exists);
        }
        catch (Exception ex)
        {
            return HandleException<bool>(ex, "la verifica dell'esistenza", id);
        }
    }
}
