namespace GameStore.Shared.DTOs;

/// <summary>
/// DTO per visualizzare i giochi acquistati con informazioni dell'utente e del gioco
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
    /// Prezzo del gioco
    /// </summary>
    public decimal GiocoPrezzo { get; set; }

    /// <summary>
    /// Data dell'acquisto
    /// </summary>
    public DateTime DataAcquisto { get; set; }

    /// <summary>
    /// Data di creazione dell'acquisto
    /// </summary>
    public DateTime DataCreazione { get; set; }
}
