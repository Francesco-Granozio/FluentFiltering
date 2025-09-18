namespace GameStore.Domain.Entities;

/// <summary>
/// Entità Recensione rappresentante una recensione di un gioco da parte di un utente
/// </summary>
public class Recensione : IEntity<Guid>, IAuditable, ISoftDelete
{
    public Guid Id { get; set; }
    
    /// <summary>
    /// ID dell'utente che ha scritto la recensione
    /// </summary>
    public Guid UtenteId { get; set; }
    
    /// <summary>
    /// ID del gioco recensito
    /// </summary>
    public Guid GiocoId { get; set; }
    
    /// <summary>
    /// Punteggio assegnato al gioco (range 1-5)
    /// </summary>
    public int Punteggio { get; set; }
    
    /// <summary>
    /// Titolo della recensione (opzionale, max 200 caratteri)
    /// </summary>
    public string? Titolo { get; set; }
    
    /// <summary>
    /// Corpo della recensione (testo lungo)
    /// </summary>
    public string? Corpo { get; set; }
    
    /// <summary>
    /// Data e ora della recensione
    /// </summary>
    public DateTime DataRecensione { get; set; }
    
    /// <summary>
    /// Indica se la recensione è verificata (l'utente ha effettivamente acquistato il gioco)
    /// </summary>
    public bool IsRecensioneVerificata { get; set; }
    
    /// <summary>
    /// ID dell'acquisto verificato (opzionale, nullable)
    /// </summary>
    public Guid? AcquistoId { get; set; }
    
    // Implementazione IAuditable
    public DateTime DataCreazione { get; set; }
    public DateTime? DataUltimaModifica { get; set; }
    
    // Implementazione ISoftDelete
    public bool IsCancellato { get; set; }
    public DateTime? DataCancellazione { get; set; }
    
    // Navigazioni
    public Utente Utente { get; set; } = null!;
    public Gioco Gioco { get; set; } = null!;
    public Acquisto? Acquisto { get; set; }
}
