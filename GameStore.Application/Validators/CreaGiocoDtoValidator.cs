using FluentValidation;
using GameStore.Application.DTOs;

namespace GameStore.Application.Validators;

/// <summary>
/// Validatore per CreaGiocoDto
/// </summary>
public class CreaGiocoDtoValidator : AbstractValidator<CreaGiocoDto>
{
    public CreaGiocoDtoValidator()
    {
        RuleFor(x => x.Titolo)
            .NotEmpty().WithMessage("Il titolo è obbligatorio")
            .Length(1, 200).WithMessage("Il titolo deve essere tra 1 e 200 caratteri");

        RuleFor(x => x.Descrizione)
            .MaximumLength(4000).WithMessage("La descrizione non può superare i 4000 caratteri")
            .When(x => !string.IsNullOrEmpty(x.Descrizione));

        RuleFor(x => x.PrezzoListino)
            .GreaterThanOrEqualTo(0).WithMessage("Il prezzo deve essere maggiore o uguale a 0")
            .LessThanOrEqualTo(999.99m).WithMessage("Il prezzo non può superare 999.99");

        RuleFor(x => x.DataRilascio)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("La data di rilascio non può essere nel futuro")
            .When(x => x.DataRilascio.HasValue);

        RuleFor(x => x.Genere)
            .MaximumLength(100).WithMessage("Il genere non può superare i 100 caratteri")
            .When(x => !string.IsNullOrEmpty(x.Genere));

        RuleFor(x => x.Piattaforma)
            .MaximumLength(50).WithMessage("La piattaforma non può superare i 50 caratteri")
            .When(x => !string.IsNullOrEmpty(x.Piattaforma));

        RuleFor(x => x.Sviluppatore)
            .MaximumLength(150).WithMessage("Lo sviluppatore non può superare i 150 caratteri")
            .When(x => !string.IsNullOrEmpty(x.Sviluppatore));
    }
}
