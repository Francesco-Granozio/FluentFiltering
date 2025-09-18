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
    public string Titolo { get; set; } = string.Empty;
    public string? Descrizione { get; set; }
    public decimal PrezzoListino { get; set; }
    public DateTime? DataRilascio { get; set; }
    public string? Genere { get; set; }
    public string? Piattaforma { get; set; }
    public string? Sviluppatore { get; set; }
}

/// <summary>
/// DTO per l'aggiornamento di un gioco
/// </summary>
public class AggiornaGiocoDto
{
    public Guid Id { get; set; }
    public string Titolo { get; set; } = string.Empty;
    public string? Descrizione { get; set; }
    public decimal PrezzoListino { get; set; }
    public DateTime? DataRilascio { get; set; }
    public string? Genere { get; set; }
    public string? Piattaforma { get; set; }
    public string? Sviluppatore { get; set; }
}
