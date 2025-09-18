namespace GameStore.Domain.Entities;

/// <summary>
/// Entità Utente rappresentante un utente del sistema
/// </summary>
public class Utente : IEntity<Guid>, IAuditable, ISoftDelete
{
    public Guid Id { get; set; }
    
    /// <summary>
    /// Username univoco dell'utente (max 50 caratteri)
    /// </summary>
    public string Username { get; set; } = string.Empty;
    
    /// <summary>
    /// Email univoca dell'utente (max 254 caratteri)
    /// </summary>
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// Nome completo dell'utente (max 200 caratteri)
    /// </summary>
    public string NomeCompleto { get; set; } = string.Empty;
    
    /// <summary>
    /// Paese di residenza dell'utente (opzionale, max 100 caratteri)
    /// </summary>
    public string? Paese { get; set; }
    
    /// <summary>
    /// Data di registrazione dell'utente
    /// </summary>
    public DateTime DataRegistrazione { get; set; }
    
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
