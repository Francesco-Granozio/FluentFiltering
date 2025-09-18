namespace GameStore.Application.Common;

/// <summary>
/// Classe che rappresenta un errore dell'applicazione
/// </summary>
public class Error
{
    public string Code { get; }
    public string Message { get; }

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public static Error Create(string code, string message) => new(code, message);

    public static Error NotFound(string entityName) => 
        new("NOT_FOUND", $"{entityName} non trovato");

    public static Error ValidationFailed(string message) => 
        new("VALIDATION_FAILED", message);

    public static Error DatabaseError(string message) => 
        new("DATABASE_ERROR", message);

    public static Error UnexpectedError(string message) => 
        new("UNEXPECTED_ERROR", message);

    public static Error Unauthorized() => 
        new("UNAUTHORIZED", "Non autorizzato");
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
        public static readonly Error UsernameAlreadyExists = Error.Create("USERNAME_EXISTS", "Username già utilizzato");
        public static readonly Error EmailAlreadyExists = Error.Create("EMAIL_EXISTS", "Email già utilizzata");
        public static readonly Error InvalidCredentials = Error.Create("INVALID_CREDENTIALS", "Credenziali non valide");
    }

    /// <summary>
    /// Errori specifici per Giochi
    /// </summary>
    public static class Giochi
    {
        public static readonly Error NotFound = Error.NotFound("Gioco");
        public static readonly Error InvalidPrice = Error.Create("INVALID_PRICE", "Prezzo non valido");
        public static readonly Error InvalidReleaseDate = Error.Create("INVALID_RELEASE_DATE", "Data di rilascio non valida");
    }

    /// <summary>
    /// Errori specifici per Acquisti
    /// </summary>
    public static class Acquisti
    {
        public static readonly Error NotFound = Error.NotFound("Acquisto");
        public static readonly Error UserNotFound = Error.NotFound("Utente");
        public static readonly Error GameNotFound = Error.NotFound("Gioco");
        public static readonly Error InvalidQuantity = Error.Create("INVALID_QUANTITY", "Quantità non valida");
        public static readonly Error InvalidPrice = Error.Create("INVALID_PRICE", "Prezzo non valido");
    }

    /// <summary>
    /// Errori specifici per Recensioni
    /// </summary>
    public static class Recensioni
    {
        public static readonly Error NotFound = Error.NotFound("Recensione");
        public static readonly Error UserNotFound = Error.NotFound("Utente");
        public static readonly Error GameNotFound = Error.NotFound("Gioco");
        public static readonly Error DuplicateReview = Error.Create("DUPLICATE_REVIEW", "Recensione duplicata per questo utente e gioco");
        public static readonly Error InvalidScore = Error.Create("INVALID_SCORE", "Punteggio non valido (deve essere tra 1 e 5)");
        public static readonly Error InvalidPurchase = Error.Create("INVALID_PURCHASE", "Acquisto non valido per questa recensione");
    }
}
