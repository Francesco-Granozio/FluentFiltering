using FluentValidation;
using GameStore.Application.DTOs;

namespace GameStore.Application.Validators;

/// <summary>
/// Validatore per AggiornaRecensioneDto
/// </summary>
public class AggiornaRecensioneDtoValidator : AbstractValidator<AggiornaRecensioneDto>
{
    public AggiornaRecensioneDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("L'ID è obbligatorio");

        RuleFor(x => x.UtenteId)
            .NotEmpty().WithMessage("L'ID utente è obbligatorio");

        RuleFor(x => x.GiocoId)
            .NotEmpty().WithMessage("L'ID gioco è obbligatorio");

        RuleFor(x => x.Punteggio)
            .InclusiveBetween(1, 5).WithMessage("Il punteggio deve essere tra 1 e 5");

        RuleFor(x => x.Titolo)
            .NotEmpty().WithMessage("Il titolo è obbligatorio")
            .MaximumLength(200).WithMessage("Il titolo non può superare i 200 caratteri");

        RuleFor(x => x.Corpo)
            .MaximumLength(2000).WithMessage("Il corpo della recensione non può superare i 2000 caratteri");

        RuleFor(x => x.DataRecensione)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("La data di recensione non può essere futura")
            .GreaterThan(DateTime.Now.AddYears(-5)).WithMessage("La data di recensione non può essere troppo nel passato");
    }
}
