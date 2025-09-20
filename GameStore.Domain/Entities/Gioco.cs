namespace GameStore.Domain.Entities;

/// <summary>
/// Entità Gioco rappresentante un videogioco nel catalogo
/// </summary>
public class Gioco : IEntity<Guid>, IAuditable, ISoftDelete
{
    public Guid Id { get; set; }

    /// <summary>
    /// Titolo del gioco (max 200 caratteri)
    /// </summary>
    public string Titolo { get; set; } = string.Empty;

    /// <summary>
    /// Descrizione dettagliata del gioco (opzionale)
    /// </summary>
    public string? Descrizione { get; set; }

    /// <summary>
    /// Prezzo di listino del gioco (decimal 18,2)
    /// </summary>
    public decimal PrezzoListino { get; set; }

    /// <summary>
    /// Data di rilascio del gioco (opzionale)
    /// </summary>
    public DateTime? DataRilascio { get; set; }

    /// <summary>
    /// Genere del gioco (max 100 caratteri)
    /// </summary>
    public string? Genere { get; set; }

    /// <summary>
    /// Piattaforma di gioco (max 50 caratteri, es. "PC", "Xbox", "PS")
    /// </summary>
    public string? Piattaforma { get; set; }

    /// <summary>
    /// Nome dello sviluppatore (max 150 caratteri)
    /// </summary>
    public string? Sviluppatore { get; set; }

    // Implementazione IAuditable
    public DateTime DataCreazione { get; set; }
    public DateTime? DataUltimaModifica { get; set; }

    // Implementazione ISoftDelete
    public bool IsCancellato { get; set; }
    public DateTime? DataCancellazione { get; set; }

    // Navigazioni
    public ICollection<Acquisto> Acquisti { get; set; } = new List<Acquisto>();
    public ICollection<Recensione> Recensioni { get; set; } = new List<Recensione>();
}
