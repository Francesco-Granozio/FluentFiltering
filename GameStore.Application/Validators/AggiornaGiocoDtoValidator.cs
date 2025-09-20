using FluentValidation;

namespace GameStore.Application.Validators;

/// <summary>
/// Validatore per AggiornaGiocoDto
/// </summary>
public class AggiornaGiocoDtoValidator : AbstractValidator<AggiornaGiocoDto>
{
    public AggiornaGiocoDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("L'ID è obbligatorio");

        RuleFor(x => x.Titolo)
            .NotEmpty().WithMessage("Il titolo è obbligatorio")
            .MaximumLength(200).WithMessage("Il titolo non può superare i 200 caratteri");

        RuleFor(x => x.Descrizione)
            .MaximumLength(2000).WithMessage("La descrizione non può superare i 2000 caratteri");

        RuleFor(x => x.PrezzoListino)
            .GreaterThanOrEqualTo(0).WithMessage("Il prezzo deve essere maggiore o uguale a 0")
            .LessThan(10000).WithMessage("Il prezzo non può superare 10000");

        RuleFor(x => x.DataRilascio)
            .LessThanOrEqualTo(DateTime.Today).WithMessage("La data di rilascio non può essere futura");

        RuleFor(x => x.Genere)
            .NotEmpty().WithMessage("Il genere è obbligatorio")
            .MaximumLength(100).WithMessage("Il genere non può superare i 100 caratteri");

        RuleFor(x => x.Piattaforma)
            .NotEmpty().WithMessage("La piattaforma è obbligatoria")
            .MaximumLength(100).WithMessage("La piattaforma non può superare i 100 caratteri");

        RuleFor(x => x.Sviluppatore)
            .NotEmpty().WithMessage("Lo sviluppatore è obbligatorio")
            .MaximumLength(200).WithMessage("Lo sviluppatore non può superare i 200 caratteri");
    }
}
