using AutoMapper;
using GameStore.Application.Common;
using GameStore.Application.DTOs;
using GameStore.Domain.DTOs.Common;
using GameStore.Domain.Interfaces;
using GameStore.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace GameStore.Application.Services;

/// <summary>
/// Implementazione del servizio per la gestione degli acquisti
/// </summary>
public class AcquistoService : IAcquistoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<AcquistoService> _logger;

    public AcquistoService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AcquistoService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<AcquistoDto>> GetByIdAsync(Guid id, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var acquisto = await _unitOfWork.Acquisti.GetByIdAsync(id, includeDeleted, cancellationToken);
        if (acquisto == null)
        {
            _logger.LogWarning("Acquisto con ID {AcquistoId} non trovato.", id);
            return Result<AcquistoDto>.Failure(Errors.Acquisti.NotFound);
        }
        return _mapper.Map<AcquistoDto>(acquisto);
    }

    public async Task<Result<PagedResult<AcquistoDto>>> GetPagedAsync(FilterRequest request, CancellationToken cancellationToken = default)
    {
        var pagedResult = await _unitOfWork.Acquisti.GetPagedAsync(request, cancellationToken: cancellationToken);
        var dtoList = _mapper.Map<IEnumerable<AcquistoDto>>(pagedResult.Items);

        return new PagedResult<AcquistoDto>
        {
            Items = dtoList,
            TotalItems = pagedResult.TotalItems,
            PageNumber = pagedResult.PageNumber,
            PageSize = pagedResult.PageSize,
            TotalPages = pagedResult.TotalPages
        };
    }

    public async Task<Result<AcquistoDto>> CreateAsync(CreaAcquistoDto dto, CancellationToken cancellationToken = default)
    {
        // Verifica che l'utente esista
        var utente = await _unitOfWork.Utenti.GetByIdAsync(dto.UtenteId, false, cancellationToken);
        if (utente == null)
        {
            return Result<AcquistoDto>.Failure(Errors.Acquisti.UserNotFound);
        }

        // Verifica che il gioco esista
        var gioco = await _unitOfWork.Giochi.GetByIdAsync(dto.GiocoId, false, cancellationToken);
        if (gioco == null)
        {
            return Result<AcquistoDto>.Failure(Errors.Acquisti.GameNotFound);
        }

        var acquisto = _mapper.Map<Acquisto>(dto);
        await _unitOfWork.Acquisti.AddAsync(acquisto, cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Acquisto creato con ID {AcquistoId} per utente {UtenteId} e gioco {GiocoId}.", 
            acquisto.Id, acquisto.UtenteId, acquisto.GiocoId);
        return _mapper.Map<AcquistoDto>(acquisto);
    }

    public async Task<Result<AcquistoDto>> UpdateAsync(AggiornaAcquistoDto dto, CancellationToken cancellationToken = default)
    {
        var acquisto = await _unitOfWork.Acquisti.GetByIdAsync(dto.Id, false, cancellationToken);
        if (acquisto == null)
        {
            _logger.LogWarning("Aggiornamento fallito: Acquisto con ID {AcquistoId} non trovato.", dto.Id);
            return Result<AcquistoDto>.Failure(Errors.Acquisti.NotFound);
        }

        // Verifica che l'utente esista se è stato cambiato
        if (acquisto.UtenteId != dto.UtenteId)
        {
            var utente = await _unitOfWork.Utenti.GetByIdAsync(dto.UtenteId, false, cancellationToken);
            if (utente == null)
            {
                return Result<AcquistoDto>.Failure(Errors.Acquisti.UserNotFound);
            }
        }

        // Verifica che il gioco esista se è stato cambiato
        if (acquisto.GiocoId != dto.GiocoId)
        {
            var gioco = await _unitOfWork.Giochi.GetByIdAsync(dto.GiocoId, false, cancellationToken);
            if (gioco == null)
            {
                return Result<AcquistoDto>.Failure(Errors.Acquisti.GameNotFound);
            }
        }

        _mapper.Map(dto, acquisto);
        _unitOfWork.Acquisti.Update(acquisto);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Acquisto con ID {AcquistoId} aggiornato.", acquisto.Id);
        return _mapper.Map<AcquistoDto>(acquisto);
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var acquisto = await _unitOfWork.Acquisti.GetByIdAsync(id, false, cancellationToken);
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
        var exists = await _unitOfWork.Acquisti.ExistsAsync(id, false, cancellationToken);
        return exists;
    }

    public async Task<Result<IEnumerable<AcquistoDto>>> GetByUtenteAsync(Guid utenteId, CancellationToken cancellationToken = default)
    {
        var acquisti = await _unitOfWork.Acquisti.GetByUtenteIdAsync(utenteId, false, cancellationToken);
        var dtoList = _mapper.Map<IEnumerable<AcquistoDto>>(acquisti);
        return Result<IEnumerable<AcquistoDto>>.Success(dtoList);
    }
}
