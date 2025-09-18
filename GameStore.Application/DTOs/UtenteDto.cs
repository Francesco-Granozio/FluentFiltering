using System.ComponentModel.DataAnnotations;

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
    [Required(ErrorMessage = "Lo username è obbligatorio")]
    [StringLength(50, ErrorMessage = "Lo username non può superare i 50 caratteri")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "L'email è obbligatoria")]
    [EmailAddress(ErrorMessage = "L'email non è valida")]
    [StringLength(100, ErrorMessage = "L'email non può superare i 100 caratteri")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Il nome completo è obbligatorio")]
    [StringLength(150, ErrorMessage = "Il nome completo non può superare i 150 caratteri")]
    public string NomeCompleto { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "Il paese non può superare i 50 caratteri")]
    public string? Paese { get; set; }

    public DateTime DataRegistrazione { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// DTO per l'aggiornamento di un utente
/// </summary>
public class AggiornaUtenteDto
{
    [Required(ErrorMessage = "L'ID è obbligatorio")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Lo username è obbligatorio")]
    [StringLength(50, ErrorMessage = "Lo username non può superare i 50 caratteri")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "L'email è obbligatoria")]
    [EmailAddress(ErrorMessage = "L'email non è valida")]
    [StringLength(100, ErrorMessage = "L'email non può superare i 100 caratteri")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Il nome completo è obbligatorio")]
    [StringLength(150, ErrorMessage = "Il nome completo non può superare i 150 caratteri")]
    public string NomeCompleto { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "Il paese non può superare i 50 caratteri")]
    public string? Paese { get; set; }
}
