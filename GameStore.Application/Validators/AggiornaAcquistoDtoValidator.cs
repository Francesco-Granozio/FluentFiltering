using FluentValidation;
using GameStore.Application.DTOs;

namespace GameStore.Application.Validators;

/// <summary>
/// Validatore per AggiornaAcquistoDto
/// </summary>
public class AggiornaAcquistoDtoValidator : AbstractValidator<AggiornaAcquistoDto>
{
    public AggiornaAcquistoDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("L'ID è obbligatorio");

        RuleFor(x => x.UtenteId)
            .NotEmpty().WithMessage("L'ID utente è obbligatorio");

        RuleFor(x => x.GiocoId)
            .NotEmpty().WithMessage("L'ID gioco è obbligatorio");

        RuleFor(x => x.DataAcquisto)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("La data di acquisto non può essere futura")
            .GreaterThan(DateTime.Now.AddYears(-10)).WithMessage("La data di acquisto non può essere troppo nel passato");

        RuleFor(x => x.PrezzoPagato)
            .GreaterThan(0).WithMessage("Il prezzo pagato deve essere maggiore di 0")
            .LessThan(10000).WithMessage("Il prezzo pagato non può superare 10000");

        RuleFor(x => x.Quantita)
            .GreaterThan(0).WithMessage("La quantità deve essere maggiore di 0")
            .LessThanOrEqualTo(10).WithMessage("La quantità non può superare 10");

        RuleFor(x => x.MetodoPagamento)
            .NotEmpty().WithMessage("Il metodo di pagamento è obbligatorio")
            .MaximumLength(50).WithMessage("Il metodo di pagamento non può superare i 50 caratteri");

        RuleFor(x => x.CodiceSconto)
            .MaximumLength(50).WithMessage("Il codice sconto non può superare i 50 caratteri");
    }
}
