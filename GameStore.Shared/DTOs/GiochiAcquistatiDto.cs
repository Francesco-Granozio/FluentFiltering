namespace GameStore.Shared.DTOs;

/// <summary>
/// DTO per visualizzare i giochi acquistati con informazioni complete dell'utente, acquisto e gioco
/// </summary>
public class GiochiAcquistatiDto
{
    /// <summary>
    /// ID dell'acquisto
    /// </summary>
    public Guid AcquistoId { get; set; }

    /// <summary>
    /// ID dell'utente
    /// </summary>
    public Guid UtenteId { get; set; }

    /// <summary>
    /// Username dell'utente
    /// </summary>
    public string UtenteUsername { get; set; } = string.Empty;

    /// <summary>
    /// Email dell'utente
    /// </summary>
    public string UtenteEmail { get; set; } = string.Empty;

    /// <summary>
    /// Nome completo dell'utente
    /// </summary>
    public string UtenteNomeCompleto { get; set; } = string.Empty;

    /// <summary>
    /// Data dell'acquisto
    /// </summary>
    public DateTime DataAcquisto { get; set; }

    /// <summary>
    /// Prezzo pagato per l'acquisto
    /// </summary>
    public decimal PrezzoPagato { get; set; }

    /// <summary>
    /// Quantità acquistata
    /// </summary>
    public int Quantita { get; set; }

    /// <summary>
    /// Metodo di pagamento utilizzato
    /// </summary>
    public string MetodoPagamento { get; set; } = string.Empty;

    /// <summary>
    /// Codice sconto utilizzato (se applicabile)
    /// </summary>
    public string CodiceSconto { get; set; } = string.Empty;

    /// <summary>
    /// ID del gioco
    /// </summary>
    public Guid GiocoId { get; set; }

    /// <summary>
    /// Titolo del gioco
    /// </summary>
    public string GiocoTitolo { get; set; } = string.Empty;

    /// <summary>
    /// Descrizione del gioco
    /// </summary>
    public string GiocoDescrizione { get; set; } = string.Empty;

    /// <summary>
    /// Prezzo di listino del gioco
    /// </summary>
    public decimal GiocoPrezzoListino { get; set; }

    /// <summary>
    /// Data di rilascio del gioco
    /// </summary>
    public DateTime? GiocoDataRilascio { get; set; }

    /// <summary>
    /// Genere del gioco
    /// </summary>
    public string GiocoGenere { get; set; } = string.Empty;

    /// <summary>
    /// Piattaforma del gioco
    /// </summary>
    public string GiocoPiattaforma { get; set; } = string.Empty;

    /// <summary>
    /// Sviluppatore del gioco
    /// </summary>
    public string GiocoSviluppatore { get; set; } = string.Empty;

    /// <summary>
    /// Data di creazione dell'acquisto
    /// </summary>
    public DateTime DataCreazione { get; set; }
}
