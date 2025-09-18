using System.ComponentModel.DataAnnotations;

namespace GameStore.Application.DTOs;

/// <summary>
/// DTO per la visualizzazione di un acquisto
/// </summary>
public class AcquistoDto
{
    public Guid Id { get; set; }
    public Guid UtenteId { get; set; }
    public Guid GiocoId { get; set; }
    public DateTime DataAcquisto { get; set; }
    public decimal PrezzoPagato { get; set; }
    public int Quantita { get; set; }
    public string? MetodoPagamento { get; set; }
    public string? CodiceSconto { get; set; }
    public DateTime DataCreazione { get; set; }
    public DateTime? DataUltimaModifica { get; set; }
    
    // Proprietà di navigazione per visualizzazione
    public string? UtenteUsername { get; set; }
    public string? GiocoTitolo { get; set; }
}

/// <summary>
/// DTO per la creazione di un acquisto
/// </summary>
public class CreaAcquistoDto
{
    [Required(ErrorMessage = "L'ID utente è obbligatorio")]
    public Guid UtenteId { get; set; }

    [Required(ErrorMessage = "L'ID gioco è obbligatorio")]
    public Guid GiocoId { get; set; }

    [Required(ErrorMessage = "La data di acquisto è obbligatoria")]
    public DateTime DataAcquisto { get; set; } = DateTime.UtcNow;

    [Required(ErrorMessage = "Il prezzo pagato è obbligatorio")]
    [Range(0, double.MaxValue, ErrorMessage = "Il prezzo pagato non può essere negativo")]
    public decimal PrezzoPagato { get; set; }

    [Required(ErrorMessage = "La quantità è obbligatoria")]
    [Range(1, int.MaxValue, ErrorMessage = "La quantità deve essere maggiore di zero")]
    public int Quantita { get; set; } = 1;

    [StringLength(50, ErrorMessage = "Il metodo di pagamento non può superare i 50 caratteri")]
    public string? MetodoPagamento { get; set; }

    [StringLength(50, ErrorMessage = "Il codice sconto non può superare i 50 caratteri")]
    public string? CodiceSconto { get; set; }
}

/// <summary>
/// DTO per l'aggiornamento di un acquisto
/// </summary>
public class AggiornaAcquistoDto
{
    [Required(ErrorMessage = "L'ID è obbligatorio")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "L'ID utente è obbligatorio")]
    public Guid UtenteId { get; set; }

    [Required(ErrorMessage = "L'ID gioco è obbligatorio")]
    public Guid GiocoId { get; set; }

    [Required(ErrorMessage = "La data di acquisto è obbligatoria")]
    public DateTime DataAcquisto { get; set; }

    [Required(ErrorMessage = "Il prezzo pagato è obbligatorio")]
    [Range(0, double.MaxValue, ErrorMessage = "Il prezzo pagato non può essere negativo")]
    public decimal PrezzoPagato { get; set; }

    [Required(ErrorMessage = "La quantità è obbligatoria")]
    [Range(1, int.MaxValue, ErrorMessage = "La quantità deve essere maggiore di zero")]
    public int Quantita { get; set; }

    [StringLength(50, ErrorMessage = "Il metodo di pagamento non può superare i 50 caratteri")]
    public string? MetodoPagamento { get; set; }

    [StringLength(50, ErrorMessage = "Il codice sconto non può superare i 50 caratteri")]
    public string? CodiceSconto { get; set; }
}
