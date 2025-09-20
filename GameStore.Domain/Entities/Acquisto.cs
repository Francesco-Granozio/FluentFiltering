namespace GameStore.Domain.Entities;

/// <summary>
/// Entità Acquisto rappresentante l'acquisto di un gioco da parte di un utente
/// </summary>
public class Acquisto : IEntity<Guid>, IAuditable, ISoftDelete
{
    public Guid Id { get; set; }

    /// <summary>
    /// ID dell'utente che ha effettuato l'acquisto
    /// </summary>
    public Guid UtenteId { get; set; }

    /// <summary>
    /// ID del gioco acquistato
    /// </summary>
    public Guid GiocoId { get; set; }

    /// <summary>
    /// Data e ora dell'acquisto
    /// </summary>
    public DateTime DataAcquisto { get; set; }

    /// <summary>
    /// Prezzo effettivamente pagato (decimal 18,2)
    /// </summary>
    public decimal PrezzoPagato { get; set; }

    /// <summary>
    /// Quantità acquistata (default 1)
    /// </summary>
    public int Quantita { get; set; } = 1;

    /// <summary>
    /// Metodo di pagamento utilizzato (max 50 caratteri, es. "Carta", "PayPal")
    /// </summary>
    public string? MetodoPagamento { get; set; }

    /// <summary>
    /// Codice sconto applicato (opzionale, max 50 caratteri)
    /// </summary>
    public string? CodiceSconto { get; set; }

    // Implementazione IAuditable
    public DateTime DataCreazione { get; set; }
    public DateTime? DataUltimaModifica { get; set; }

    // Implementazione ISoftDelete
    public bool IsCancellato { get; set; }
    public DateTime? DataCancellazione { get; set; }

    // Navigazioni
    public Utente Utente { get; set; } = null!;
    public Gioco Gioco { get; set; } = null!;
    public ICollection<Recensione> Recensioni { get; set; } = new List<Recensione>();
}
