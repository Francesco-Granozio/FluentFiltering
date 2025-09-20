using System.Globalization;
using System.Text.RegularExpressions;

namespace GameStore.Infrastructure.Common;

/// <summary>
/// Helper per il filtraggio dinamico sicuro con LINQ
/// </summary>
public static class DynamicLinqHelper
{
    /// <summary>
    /// Valida e sanifica un filtro dinamico
    /// </summary>
    /// <param name="filter">Filtro da validare</param>
    /// <param name="entityType">Tipo dell'entità</param>
    /// <returns>Filtro validato e sanificato</returns>
    public static string ValidateAndSanitizeFilter(string filter, Type entityType)
    {
        if (string.IsNullOrWhiteSpace(filter))
            return string.Empty;

        // Rimuovi caratteri pericolosi
        string sanitizedFilter = Regex.Replace(filter, @"[;{}]", "");

        // Converti le date dal formato italiano (dd/MM/yyyy) al formato standard (yyyy-MM-dd)
        sanitizedFilter = ConvertItalianDatesToStandard(sanitizedFilter);

        return sanitizedFilter;
    }

    /// <summary>
    /// Converte le date dal formato italiano al formato standard per Dynamic LINQ
    /// </summary>
    /// <param name="filter">Filtro contenente potenziali date</param>
    /// <returns>Filtro con date convertite</returns>
    private static string ConvertItalianDatesToStandard(string filter)
    {
        // Pattern per riconoscere confronti di uguaglianza con date italiane
        // Es: DataRegistrazione == "15/09/2025 00:00:00"
        string dateEqualityPattern = @"(\w+)\s*==\s*(['""])(\d{1,2}/\d{1,2}/\d{4})(?:\s+\d{1,2}:\d{2}:\d{2})?\2";

        return Regex.Replace(filter, dateEqualityPattern, match =>
        {
            string fieldName = match.Groups[1].Value; // Nome del campo (es: DataRegistrazione)
            string quote = match.Groups[2].Value; // Virgolette usate
            string italianDateStr = match.Groups[3].Value; // Solo la parte data (dd/MM/yyyy)

            // Prova a parsare la data in formato italiano
            if (DateTime.TryParseExact(italianDateStr,
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime date))
            {
                // Converte in un range che copre tutto il giorno
                DateTime startOfDay = date.Date;
                DateTime endOfDay = date.Date.AddDays(1);

                // Sostituisce con un range: field >= startOfDay AND field < endOfDay
                return $"({fieldName} >= {quote}{startOfDay:yyyy-MM-dd}T00:00:00.0000000{quote} AND {fieldName} < {quote}{endOfDay:yyyy-MM-dd}T00:00:00.0000000{quote})";
            }

            // Se non riesce a parsare, lascia il valore originale
            return match.Value;
        });
    }

    /// <summary>
    /// Valida e sanifica un ordinamento dinamico
    /// </summary>
    /// <param name="orderBy">Ordinamento da validare</param>
    /// <param name="entityType">Tipo dell'entità</param>
    /// <returns>Ordinamento validato e sanificato</returns>
    public static string ValidateAndSanitizeOrderBy(string orderBy, Type entityType)
    {
        if (string.IsNullOrWhiteSpace(orderBy))
            return string.Empty;

        // Ottieni la whitelist per il tipo di entità
        HashSet<string> whitelist = GetWhitelistForType(entityType);

        // Rimuovi caratteri pericolosi, mantieni solo campi, virgole e asc/desc
        string sanitizedOrderBy = Regex.Replace(orderBy, @"[^\w\s,]", "");

        // Valida ogni campo nell'ordinamento
        string[] orderByParts = sanitizedOrderBy.Split(',', StringSplitOptions.RemoveEmptyEntries);
        List<string> validatedParts = new();

        foreach (string part in orderByParts)
        {
            string trimmedPart = part.Trim();
            string[] parts = trimmedPart.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0) continue;

            string fieldName = parts[0];
            if (!whitelist.Contains(fieldName, StringComparer.OrdinalIgnoreCase))
            {
                throw new ArgumentException($"Campo non consentito nell'ordinamento: {fieldName}");
            }

            // Aggiungi direzione se specificata
            string direction = parts.Length > 1 && parts[1].ToLower() == "desc" ? " desc" : " asc";
            validatedParts.Add(fieldName + direction);
        }

        return string.Join(", ", validatedParts);
    }

    /// <summary>
    /// Ottiene la whitelist per un tipo di entità
    /// </summary>
    /// <param name="entityType">Tipo dell'entità</param>
    /// <returns>HashSet con i campi consentiti</returns>
    private static HashSet<string> GetWhitelistForType(Type entityType)
    {
        return entityType.Name switch
        {
            nameof(Domain.Entities.Utente) => FieldWhitelist.UtenteFields,
            nameof(Domain.Entities.Gioco) => FieldWhitelist.GiocoFields,
            nameof(Domain.Entities.Acquisto) => FieldWhitelist.AcquistoFields,
            nameof(Domain.Entities.Recensione) => FieldWhitelist.RecensioneFields,
            _ => throw new ArgumentException($"Tipo di entità non supportato: {entityType.Name}")
        };
    }
}
