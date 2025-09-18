namespace GameStore.Application.DTOs;

/// <summary>
/// DTO per la visualizzazione di un utente
/// </summary>
public class UtenteDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string NomeCompleto { get; set; } = string.Empty;
    public string? Paese { get; set; }
    public DateTime DataRegistrazione { get; set; }
    public DateTime DataCreazione { get; set; }
    public DateTime? DataUltimaModifica { get; set; }
}

/// <summary>
/// DTO per la creazione di un utente
/// </summary>
public class CreaUtenteDto
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string NomeCompleto { get; set; } = string.Empty;
    public string? Paese { get; set; }
    public DateTime DataRegistrazione { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// DTO per l'aggiornamento di un utente
/// </summary>
public class AggiornaUtenteDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string NomeCompleto { get; set; } = string.Empty;
    public string? Paese { get; set; }
}
