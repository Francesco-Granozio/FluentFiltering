using System.ComponentModel.DataAnnotations;

namespace GameStore.Application.DTOs;

/// <summary>
/// DTO per la visualizzazione di un gioco
/// </summary>
public class GiocoDto
{
    public Guid Id { get; set; }
    public string Titolo { get; set; } = string.Empty;
    public string? Descrizione { get; set; }
    public decimal PrezzoListino { get; set; }
    public DateTime? DataRilascio { get; set; }
    public string? Genere { get; set; }
    public string? Piattaforma { get; set; }
    public string? Sviluppatore { get; set; }
    public DateTime DataCreazione { get; set; }
    public DateTime? DataUltimaModifica { get; set; }
}

/// <summary>
/// DTO per la creazione di un gioco
/// </summary>
public class CreaGiocoDto
{
    [Required(ErrorMessage = "Il titolo è obbligatorio")]
    [StringLength(100, ErrorMessage = "Il titolo non può superare i 100 caratteri")]
    public string Titolo { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "La descrizione non può superare i 1000 caratteri")]
    public string? Descrizione { get; set; }

    [Required(ErrorMessage = "Il prezzo di listino è obbligatorio")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Il prezzo di listino deve essere maggiore di zero")]
    public decimal PrezzoListino { get; set; }

    [Required(ErrorMessage = "La data di rilascio è obbligatoria")]
    public DateTime? DataRilascio { get; set; }

    [Required(ErrorMessage = "Il genere è obbligatorio")]
    [StringLength(50, ErrorMessage = "Il genere non può superare i 50 caratteri")]
    public string? Genere { get; set; }

    [Required(ErrorMessage = "La piattaforma è obbligatoria")]
    [StringLength(50, ErrorMessage = "La piattaforma non può superare i 50 caratteri")]
    public string? Piattaforma { get; set; }

    [Required(ErrorMessage = "Lo sviluppatore è obbligatorio")]
    [StringLength(100, ErrorMessage = "Lo sviluppatore non può superare i 100 caratteri")]
    public string? Sviluppatore { get; set; }
}

/// <summary>
/// DTO per l'aggiornamento di un gioco
/// </summary>
public class AggiornaGiocoDto
{
    [Required(ErrorMessage = "L'ID è obbligatorio")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Il titolo è obbligatorio")]
    [StringLength(100, ErrorMessage = "Il titolo non può superare i 100 caratteri")]
    public string Titolo { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "La descrizione non può superare i 1000 caratteri")]
    public string? Descrizione { get; set; }

    [Required(ErrorMessage = "Il prezzo di listino è obbligatorio")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Il prezzo di listino deve essere maggiore di zero")]
    public decimal PrezzoListino { get; set; }

    [Required(ErrorMessage = "La data di rilascio è obbligatoria")]
    public DateTime? DataRilascio { get; set; }

    [Required(ErrorMessage = "Il genere è obbligatorio")]
    [StringLength(50, ErrorMessage = "Il genere non può superare i 50 caratteri")]
    public string? Genere { get; set; }

    [Required(ErrorMessage = "La piattaforma è obbligatoria")]
    [StringLength(50, ErrorMessage = "La piattaforma non può superare i 50 caratteri")]
    public string? Piattaforma { get; set; }

    [Required(ErrorMessage = "Lo sviluppatore è obbligatorio")]
    [StringLength(100, ErrorMessage = "Lo sviluppatore non può superare i 100 caratteri")]
    public string? Sviluppatore { get; set; }
}
