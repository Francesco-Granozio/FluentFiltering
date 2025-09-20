using Microsoft.Extensions.Logging;

namespace GameStore.Application.Services;

/// <summary>
/// Implementazione del servizio per la gestione degli utenti
/// </summary>
public class UtenteService : IUtenteService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UtenteService> _logger;

    public UtenteService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UtenteService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<UtenteDto>> GetByIdAsync(Guid id, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        try
        {
            Domain.Entities.Utente? utente = await _unitOfWork.Utenti.GetByIdAsync(id, includeDeleted, cancellationToken);

            if (utente == null)
                return Result<UtenteDto>.Failure(Errors.Utenti.NotFound);

            UtenteDto dto = _mapper.Map<UtenteDto>(utente);
            return Result<UtenteDto>.Success(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero dell'utente {UtenteId}", id);
            return Result<UtenteDto>.Failure(Errors.General.DatabaseError);
        }
    }

    public async Task<Result<PagedResult<UtenteDto>>> GetPagedAsync(FilterRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            PagedResult<Domain.Entities.Utente> pagedResult = await _unitOfWork.Utenti.GetPagedAsync(request, null, null, cancellationToken);
            PagedResult<UtenteDto> dtoResult = new()
            {
                Items = _mapper.Map<IEnumerable<UtenteDto>>(pagedResult.Items),
                TotalItems = pagedResult.TotalItems,
                PageSize = pagedResult.PageSize,
                PageNumber = pagedResult.PageNumber
            };

            return Result<PagedResult<UtenteDto>>.Success(dtoResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero paginato degli utenti");
            return Result<PagedResult<UtenteDto>>.Failure(Errors.General.DatabaseError);
        }
    }

    public async Task<Result<UtenteDto>> CreateAsync(CreaUtenteDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            Domain.Entities.Utente utente = _mapper.Map<Domain.Entities.Utente>(dto);

            await _unitOfWork.Utenti.AddAsync(utente, cancellationToken);
            await _unitOfWork.SaveChangesAsync();

            UtenteDto resultDto = _mapper.Map<UtenteDto>(utente);
            _logger.LogInformation("Utente creato con successo: {UtenteId}", utente.Id);

            return Result<UtenteDto>.Success(resultDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante la creazione dell'utente");
            return Result<UtenteDto>.Failure(Errors.General.DatabaseError);
        }
    }

    public async Task<Result<UtenteDto>> UpdateAsync(AggiornaUtenteDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            Domain.Entities.Utente? existingUtente = await _unitOfWork.Utenti.GetByIdAsync(dto.Id, false, cancellationToken);

            if (existingUtente == null)
                return Result<UtenteDto>.Failure(Errors.Utenti.NotFound);

            _mapper.Map(dto, existingUtente);
            _unitOfWork.Utenti.Update(existingUtente);
            await _unitOfWork.SaveChangesAsync();

            UtenteDto resultDto = _mapper.Map<UtenteDto>(existingUtente);
            _logger.LogInformation("Utente aggiornato con successo: {UtenteId}", existingUtente.Id);

            return Result<UtenteDto>.Success(resultDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante l'aggiornamento dell'utente {UtenteId}", dto.Id);
            return Result<UtenteDto>.Failure(Errors.General.DatabaseError);
        }
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            Domain.Entities.Utente? utente = await _unitOfWork.Utenti.GetByIdAsync(id, false, cancellationToken);

            if (utente == null)
                return Result.Failure(Errors.Utenti.NotFound);

            _unitOfWork.Utenti.Remove(utente);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Utente cancellato con successo: {UtenteId}", id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante la cancellazione dell'utente {UtenteId}", id);
            return Result.Failure(Errors.General.DatabaseError);
        }
    }

    public async Task<Result<bool>> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            bool exists = await _unitOfWork.Utenti.ExistsAsync(x => x.Id == id, false, cancellationToken);
            return Result<bool>.Success(exists);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante la verifica dell'esistenza dell'utente {UtenteId}", id);
            return Result<bool>.Failure(Errors.General.DatabaseError);
        }
    }
}
