using FluentValidation;

namespace GameStore.Application.Validators;

/// <summary>
/// Validatore per CreaAcquistoDto
/// </summary>
public class CreaAcquistoDtoValidator : AbstractValidator<CreaAcquistoDto>
{
    private readonly IUtenteRepository _utenteRepository;
    private readonly IGiocoRepository _giocoRepository;

    public CreaAcquistoDtoValidator(IUtenteRepository utenteRepository, IGiocoRepository giocoRepository)
    {
        _utenteRepository = utenteRepository;
        _giocoRepository = giocoRepository;

        RuleFor(x => x.UtenteId)
            .NotEmpty().WithMessage("L'ID utente è obbligatorio")
            .MustAsync(UtenteExistsAsync).WithMessage("L'utente specificato non esiste");

        RuleFor(x => x.GiocoId)
            .NotEmpty().WithMessage("L'ID gioco è obbligatorio")
            .MustAsync(GiocoExistsAsync).WithMessage("Il gioco specificato non esiste");

        RuleFor(x => x.DataAcquisto)
            .NotEmpty().WithMessage("La data di acquisto è obbligatoria")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("La data di acquisto non può essere nel futuro");

        RuleFor(x => x.PrezzoPagato)
            .GreaterThan(0).WithMessage("Il prezzo pagato deve essere maggiore di 0")
            .LessThanOrEqualTo(999.99m).WithMessage("Il prezzo pagato non può superare 999.99");

        RuleFor(x => x.Quantita)
            .GreaterThan(0).WithMessage("La quantità deve essere maggiore di 0")
            .LessThanOrEqualTo(10).WithMessage("La quantità non può superare 10");

        RuleFor(x => x.MetodoPagamento)
            .MaximumLength(50).WithMessage("Il metodo di pagamento non può superare i 50 caratteri")
            .When(x => !string.IsNullOrEmpty(x.MetodoPagamento));

        RuleFor(x => x.CodiceSconto)
            .MaximumLength(50).WithMessage("Il codice sconto non può superare i 50 caratteri")
            .When(x => !string.IsNullOrEmpty(x.CodiceSconto));
    }

    private async Task<bool> UtenteExistsAsync(Guid utenteId, CancellationToken cancellationToken)
    {
        return await _utenteRepository.ExistsAsync(x => x.Id == utenteId, false, cancellationToken);
    }

    private async Task<bool> GiocoExistsAsync(Guid giocoId, CancellationToken cancellationToken)
    {
        return await _giocoRepository.ExistsAsync(x => x.Id == giocoId, false, cancellationToken);
    }
}
