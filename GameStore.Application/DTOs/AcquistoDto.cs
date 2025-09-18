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
    public string? UtenteNome { get; set; }
    public string? GiocoTitolo { get; set; }
}

/// <summary>
/// DTO per la creazione di un acquisto
/// </summary>
public class CreaAcquistoDto
{
    public Guid UtenteId { get; set; }
    public Guid GiocoId { get; set; }
    public DateTime DataAcquisto { get; set; } = DateTime.UtcNow;
    public decimal PrezzoPagato { get; set; }
    public int Quantita { get; set; } = 1;
    public string? MetodoPagamento { get; set; }
    public string? CodiceSconto { get; set; }
}

/// <summary>
/// DTO per l'aggiornamento di un acquisto
/// </summary>
public class AggiornaAcquistoDto
{
    public Guid Id { get; set; }
    public Guid UtenteId { get; set; }
    public Guid GiocoId { get; set; }
    public DateTime DataAcquisto { get; set; }
    public decimal PrezzoPagato { get; set; }
    public int Quantita { get; set; }
    public string? MetodoPagamento { get; set; }
    public string? CodiceSconto { get; set; }
}
