namespace GameStore.Shared.DTOs;

/// <summary>
/// DTO per la visualizzazione di una recensione
/// </summary>
public class RecensioneDto
{
    public Guid Id { get; set; }
    public Guid UtenteId { get; set; }
    public Guid GiocoId { get; set; }
    public int Punteggio { get; set; }
    public string? Titolo { get; set; }
    public string? Corpo { get; set; }
    public DateTime DataRecensione { get; set; }
    public bool IsRecensioneVerificata { get; set; }
    public Guid? AcquistoId { get; set; }
    public DateTime DataCreazione { get; set; }
    public DateTime? DataUltimaModifica { get; set; }

    // Proprietà di navigazione per visualizzazione
    public string? UtenteUsername { get; set; }
    public string? GiocoTitolo { get; set; }
}

/// <summary>
/// DTO per la creazione di una recensione
/// </summary>
public class CreaRecensioneDto
{
    [Required(ErrorMessage = "L'ID utente è obbligatorio")]
    public Guid UtenteId { get; set; }

    [Required(ErrorMessage = "L'ID gioco è obbligatorio")]
    public Guid GiocoId { get; set; }

    [Required(ErrorMessage = "Il punteggio è obbligatorio")]
    [Range(1, 5, ErrorMessage = "Il punteggio deve essere tra 1 e 5")]
    public int Punteggio { get; set; }

    [StringLength(100, ErrorMessage = "Il titolo non può superare i 100 caratteri")]
    public string? Titolo { get; set; }

    [StringLength(1000, ErrorMessage = "Il corpo della recensione non può superare i 1000 caratteri")]
    public string? Corpo { get; set; }

    [Required(ErrorMessage = "La data della recensione è obbligatoria")]
    public DateTime DataRecensione { get; set; } = DateTime.UtcNow;

    public bool IsRecensioneVerificata { get; set; } = false;

    public Guid? AcquistoId { get; set; }
}

/// <summary>
/// DTO per l'aggiornamento di una recensione
/// </summary>
public class AggiornaRecensioneDto
{
    [Required(ErrorMessage = "L'ID è obbligatorio")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "L'ID utente è obbligatorio")]
    public Guid UtenteId { get; set; }

    [Required(ErrorMessage = "L'ID gioco è obbligatorio")]
    public Guid GiocoId { get; set; }

    [Required(ErrorMessage = "Il punteggio è obbligatorio")]
    [Range(1, 5, ErrorMessage = "Il punteggio deve essere tra 1 e 5")]
    public int Punteggio { get; set; }

    [StringLength(100, ErrorMessage = "Il titolo non può superare i 100 caratteri")]
    public string? Titolo { get; set; }

    [StringLength(1000, ErrorMessage = "Il corpo della recensione non può superare i 1000 caratteri")]
    public string? Corpo { get; set; }

    [Required(ErrorMessage = "La data della recensione è obbligatoria")]
    public DateTime DataRecensione { get; set; }

    public bool IsRecensioneVerificata { get; set; }

    public Guid? AcquistoId { get; set; }
}
