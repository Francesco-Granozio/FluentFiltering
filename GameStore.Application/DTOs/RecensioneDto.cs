namespace GameStore.Application.DTOs;

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
    public string? UtenteNome { get; set; }
    public string? GiocoTitolo { get; set; }
}

/// <summary>
/// DTO per la creazione di una recensione
/// </summary>
public class CreaRecensioneDto
{
    public Guid UtenteId { get; set; }
    public Guid GiocoId { get; set; }
    public int Punteggio { get; set; }
    public string? Titolo { get; set; }
    public string? Corpo { get; set; }
    public DateTime DataRecensione { get; set; } = DateTime.UtcNow;
    public bool IsRecensioneVerificata { get; set; } = false;
    public Guid? AcquistoId { get; set; }
}

/// <summary>
/// DTO per l'aggiornamento di una recensione
/// </summary>
public class AggiornaRecensioneDto
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
}
