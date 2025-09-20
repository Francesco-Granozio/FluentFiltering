using FluentValidation;

namespace GameStore.Application.Validators;

/// <summary>
/// Validatore per CreaUtenteDto
/// </summary>
public class CreaUtenteDtoValidator : AbstractValidator<CreaUtenteDto>
{
    private readonly IUtenteRepository _utenteRepository;

    public CreaUtenteDtoValidator(IUtenteRepository utenteRepository)
    {
        _utenteRepository = utenteRepository;

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Lo username è obbligatorio")
            .Length(3, 50).WithMessage("Lo username deve essere tra 3 e 50 caratteri")
            .Matches("^[a-zA-Z0-9_]+$").WithMessage("Lo username può contenere solo lettere, numeri e underscore")
            .MustAsync(BeUniqueUsernameAsync).WithMessage("Lo username è già utilizzato");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("L'email è obbligatoria")
            .EmailAddress().WithMessage("L'email deve essere valida")
            .MaximumLength(254).WithMessage("L'email non può superare i 254 caratteri")
            .MustAsync(BeUniqueEmailAsync).WithMessage("L'email è già utilizzata");

        RuleFor(x => x.NomeCompleto)
            .NotEmpty().WithMessage("Il nome completo è obbligatorio")
            .Length(2, 200).WithMessage("Il nome completo deve essere tra 2 e 200 caratteri");

        RuleFor(x => x.Paese)
            .MaximumLength(100).WithMessage("Il paese non può superare i 100 caratteri")
            .When(x => !string.IsNullOrEmpty(x.Paese));

        RuleFor(x => x.DataRegistrazione)
            .NotEmpty().WithMessage("La data di registrazione è obbligatoria")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("La data di registrazione non può essere nel futuro");
    }

    private async Task<bool> BeUniqueUsernameAsync(string username, CancellationToken cancellationToken)
    {
        return !await _utenteRepository.UsernameExistsAsync(username, null, cancellationToken);
    }

    private async Task<bool> BeUniqueEmailAsync(string email, CancellationToken cancellationToken)
    {
        return !await _utenteRepository.EmailExistsAsync(email, null, cancellationToken);
    }
}
