namespace GameStore.Application.Common;

/// <summary>
/// Enumerato per i tipi di errore dell'applicazione
/// </summary>
public enum ErrorType
{
    Success,
    NotFound,
    ValidationFailed,
    DatabaseError,
    UnexpectedError,
    Unauthorized,
    UsernameAlreadyExists,
    EmailAlreadyExists,
    InvalidCredentials,
    InvalidPrice,
    InvalidReleaseDate,
    InvalidQuantity,
    DuplicateReview,
    InvalidScore,
    InvalidPurchase
}

/// <summary>
/// Record che rappresenta un errore dell'applicazione
/// </summary>
public record Error(ErrorType Type, string Message)
{
    public static Error Create(ErrorType type, string message) => new(type, message);

    public static Error NotFound(string entityName) => 
        new(ErrorType.NotFound, $"{entityName} non trovato");

    public static Error ValidationFailed(string message) => 
        new(ErrorType.ValidationFailed, message);

    public static Error DatabaseError(string message) => 
        new(ErrorType.DatabaseError, message);

    public static Error UnexpectedError(string message) => 
        new(ErrorType.UnexpectedError, message);

    public static Error Unauthorized() => 
        new(ErrorType.Unauthorized, "Non autorizzato");
}

/// <summary>
/// Classe statica per definire errori predefiniti dell'applicazione
/// </summary>
public static class Errors
{
    /// <summary>
    /// Errori generici
    /// </summary>
    public static class General
    {
        public static readonly Error NotFound = Error.NotFound("Entità");
        public static readonly Error ValidationFailed = Error.ValidationFailed("Validazione fallita");
        public static readonly Error UnexpectedError = Error.UnexpectedError("Errore inaspettato");
        public static readonly Error DatabaseError = Error.DatabaseError("Errore del database");
        public static readonly Error Unauthorized = Error.Unauthorized();
    }

    /// <summary>
    /// Errori specifici per Utenti
    /// </summary>
    public static class Utenti
    {
        public static readonly Error NotFound = Error.NotFound("Utente");
        public static readonly Error UsernameAlreadyExists = Error.Create(ErrorType.UsernameAlreadyExists, "Username già utilizzato");
        public static readonly Error EmailAlreadyExists = Error.Create(ErrorType.EmailAlreadyExists, "Email già utilizzata");
        public static readonly Error InvalidCredentials = Error.Create(ErrorType.InvalidCredentials, "Credenziali non valide");
    }

    /// <summary>
    /// Errori specifici per Giochi
    /// </summary>
    public static class Giochi
    {
        public static readonly Error NotFound = Error.NotFound("Gioco");
        public static readonly Error InvalidPrice = Error.Create(ErrorType.InvalidPrice, "Prezzo non valido");
        public static readonly Error InvalidReleaseDate = Error.Create(ErrorType.InvalidReleaseDate, "Data di rilascio non valida");
    }

    /// <summary>
    /// Errori specifici per Acquisti
    /// </summary>
    public static class Acquisti
    {
        public static readonly Error NotFound = Error.NotFound("Acquisto");
        public static readonly Error UserNotFound = Error.NotFound("Utente");
        public static readonly Error GameNotFound = Error.NotFound("Gioco");
        public static readonly Error InvalidQuantity = Error.Create(ErrorType.InvalidQuantity, "Quantità non valida");
        public static readonly Error InvalidPrice = Error.Create(ErrorType.InvalidPrice, "Prezzo non valido");
    }

    /// <summary>
    /// Errori specifici per Recensioni
    /// </summary>
    public static class Recensioni
    {
        public static readonly Error NotFound = Error.NotFound("Recensione");
        public static readonly Error UserNotFound = Error.NotFound("Utente");
        public static readonly Error GameNotFound = Error.NotFound("Gioco");
        public static readonly Error DuplicateReview = Error.Create(ErrorType.DuplicateReview, "Recensione duplicata per questo utente e gioco");
        public static readonly Error InvalidScore = Error.Create(ErrorType.InvalidScore, "Punteggio non valido (deve essere tra 1 e 5)");
        public static readonly Error InvalidPurchase = Error.Create(ErrorType.InvalidPurchase, "Acquisto non valido per questa recensione");
    }
}
