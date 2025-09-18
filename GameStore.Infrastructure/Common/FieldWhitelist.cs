namespace GameStore.Infrastructure.Common;

/// <summary>
/// Classe per gestire le whitelist dei campi per il filtraggio dinamico sicuro
/// </summary>
public static class FieldWhitelist
{
    /// <summary>
    /// Campi consentiti per Utente
    /// </summary>
    public static readonly HashSet<string> UtenteFields = new()
    {
        "Id", "Username", "Email", "NomeCompleto", "Paese", "DataRegistrazione", 
        "DataCreazione", "DataUltimaModifica", "IsCancellato", "DataCancellazione"
    };

    /// <summary>
    /// Campi consentiti per Gioco
    /// </summary>
    public static readonly HashSet<string> GiocoFields = new()
    {
        "Id", "Titolo", "Descrizione", "PrezzoListino", "DataRilascio", "Genere", 
        "Piattaforma", "Sviluppatore", "DataCreazione", "DataUltimaModifica", 
        "IsCancellato", "DataCancellazione"
    };

    /// <summary>
    /// Campi consentiti per Acquisto
    /// </summary>
    public static readonly HashSet<string> AcquistoFields = new()
    {
        "Id", "UtenteId", "GiocoId", "DataAcquisto", "PrezzoPagato", "Quantita", 
        "MetodoPagamento", "CodiceSconto", "DataCreazione", "DataUltimaModifica", 
        "IsCancellato", "DataCancellazione"
    };

    /// <summary>
    /// Campi consentiti per Recensione
    /// </summary>
    public static readonly HashSet<string> RecensioneFields = new()
    {
        "Id", "UtenteId", "GiocoId", "Punteggio", "Titolo", "Corpo", "DataRecensione", 
        "IsRecensioneVerificata", "AcquistoId", "DataCreazione", "DataUltimaModifica", 
        "IsCancellato", "DataCancellazione"
    };

    /// <summary>
    /// Ottiene la whitelist per un tipo di entità
    /// </summary>
    /// <typeparam name="T">Tipo dell'entità</typeparam>
    /// <returns>HashSet con i campi consentiti</returns>
    public static HashSet<string> GetWhitelistFor<T>()
    {
        var typeName = typeof(T).Name;
        return typeName switch
        {
            nameof(Domain.Entities.Utente) => UtenteFields,
            nameof(Domain.Entities.Gioco) => GiocoFields,
            nameof(Domain.Entities.Acquisto) => AcquistoFields,
            nameof(Domain.Entities.Recensione) => RecensioneFields,
            _ => throw new ArgumentException($"Tipo di entità non supportato: {typeName}")
        };
    }

    /// <summary>
    /// Verifica se un campo è consentito per un tipo di entità
    /// </summary>
    /// <typeparam name="T">Tipo dell'entità</typeparam>
    /// <param name="fieldName">Nome del campo</param>
    /// <returns>True se il campo è consentito</returns>
    public static bool IsFieldAllowed<T>(string fieldName)
    {
        var whitelist = GetWhitelistFor<T>();
        return whitelist.Contains(fieldName, StringComparer.OrdinalIgnoreCase);
    }
}
