using FluentValidation;
using GameStore.Application.DTOs;
using GameStore.Domain.Interfaces;

namespace GameStore.Application.Validators;

/// <summary>
/// Validatore per CreaRecensioneDto
/// </summary>
public class CreaRecensioneDtoValidator : AbstractValidator<CreaRecensioneDto>
{
    private readonly IUtenteRepository _utenteRepository;
    private readonly IGiocoRepository _giocoRepository;
    private readonly IRecensioneRepository _recensioneRepository;
    private readonly IAcquistoRepository _acquistoRepository;

    public CreaRecensioneDtoValidator(
        IUtenteRepository utenteRepository, 
        IGiocoRepository giocoRepository,
        IRecensioneRepository recensioneRepository,
        IAcquistoRepository acquistoRepository)
    {
        _utenteRepository = utenteRepository;
        _giocoRepository = giocoRepository;
        _recensioneRepository = recensioneRepository;
        _acquistoRepository = acquistoRepository;

        RuleFor(x => x.UtenteId)
            .NotEmpty().WithMessage("L'ID utente è obbligatorio")
            .MustAsync(UtenteExistsAsync).WithMessage("L'utente specificato non esiste");

        RuleFor(x => x.GiocoId)
            .NotEmpty().WithMessage("L'ID gioco è obbligatorio")
            .MustAsync(GiocoExistsAsync).WithMessage("Il gioco specificato non esiste");

        RuleFor(x => x.Punteggio)
            .InclusiveBetween(1, 5).WithMessage("Il punteggio deve essere tra 1 e 5");

        RuleFor(x => x.Titolo)
            .MaximumLength(200).WithMessage("Il titolo non può superare i 200 caratteri")
            .When(x => !string.IsNullOrEmpty(x.Titolo));

        RuleFor(x => x.Corpo)
            .MaximumLength(2000).WithMessage("Il corpo della recensione non può superare i 2000 caratteri")
            .When(x => !string.IsNullOrEmpty(x.Corpo));

        RuleFor(x => x.DataRecensione)
            .NotEmpty().WithMessage("La data di recensione è obbligatoria")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("La data di recensione non può essere nel futuro");

        RuleFor(x => x)
            .MustAsync(NotDuplicateReviewAsync).WithMessage("Esiste già una recensione di questo utente per questo gioco");

        RuleFor(x => x.AcquistoId)
            .MustAsync(AcquistoExistsAsync).WithMessage("L'acquisto specificato non esiste")
            .When(x => x.AcquistoId.HasValue);

        RuleFor(x => x.AcquistoId)
            .NotEmpty().WithMessage("L'ID acquisto è obbligatorio per recensioni verificate")
            .When(x => x.IsRecensioneVerificata);

        RuleFor(x => x)
            .MustAsync(VerifyAcquistoOwnershipAsync).WithMessage("L'acquisto specificato non appartiene all'utente o al gioco")
            .When(x => x.IsRecensioneVerificata && x.AcquistoId.HasValue);
    }

    private async Task<bool> UtenteExistsAsync(Guid utenteId, CancellationToken cancellationToken)
    {
        return await _utenteRepository.ExistsAsync(x => x.Id == utenteId, false, cancellationToken);
    }

    private async Task<bool> GiocoExistsAsync(Guid giocoId, CancellationToken cancellationToken)
    {
        return await _giocoRepository.ExistsAsync(x => x.Id == giocoId, false, cancellationToken);
    }

    private async Task<bool> NotDuplicateReviewAsync(CreaRecensioneDto dto, CancellationToken cancellationToken)
    {
        var existingReview = await _recensioneRepository.GetByUtenteEGiocoAsync(dto.UtenteId, dto.GiocoId, false, cancellationToken);
        return existingReview == null;
    }

    private async Task<bool> AcquistoExistsAsync(Guid? acquistoId, CancellationToken cancellationToken)
    {
        if (!acquistoId.HasValue) return true;
        return await _acquistoRepository.ExistsAsync(x => x.Id == acquistoId.Value, false, cancellationToken);
    }

    private async Task<bool> VerifyAcquistoOwnershipAsync(CreaRecensioneDto dto, CancellationToken cancellationToken)
    {
        if (!dto.AcquistoId.HasValue) return true;
        
        return await _acquistoRepository.ExistsAsync(
            x => x.Id == dto.AcquistoId.Value && 
                 x.UtenteId == dto.UtenteId && 
                 x.GiocoId == dto.GiocoId, 
            false, 
            cancellationToken);
    }
}
