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

        // Ottieni la whitelist per il tipo di entità
        var whitelist = GetWhitelistForType(entityType);

        // Rimuovi caratteri pericolosi
        var sanitizedFilter = Regex.Replace(filter, @"[^\w\s\.\(\)\[\]""'=<>!&|]", "");

        // Valida che tutti i campi utilizzati siano nella whitelist
        var fieldMatches = Regex.Matches(sanitizedFilter, @"\b[A-Za-z][A-Za-z0-9]*\b");
        foreach (Match match in fieldMatches)
        {
            var fieldName = match.Value;
            if (!whitelist.Contains(fieldName, StringComparer.OrdinalIgnoreCase))
            {
                throw new ArgumentException($"Campo non consentito: {fieldName}");
            }
        }

        return sanitizedFilter;
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
        var whitelist = GetWhitelistForType(entityType);

        // Rimuovi caratteri pericolosi, mantieni solo campi, virgole e asc/desc
        var sanitizedOrderBy = Regex.Replace(orderBy, @"[^\w\s,]", "");

        // Valida ogni campo nell'ordinamento
        var orderByParts = sanitizedOrderBy.Split(',', StringSplitOptions.RemoveEmptyEntries);
        var validatedParts = new List<string>();

        foreach (var part in orderByParts)
        {
            var trimmedPart = part.Trim();
            var parts = trimmedPart.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            
            if (parts.Length == 0) continue;

            var fieldName = parts[0];
            if (!whitelist.Contains(fieldName, StringComparer.OrdinalIgnoreCase))
            {
                throw new ArgumentException($"Campo non consentito nell'ordinamento: {fieldName}");
            }

            // Aggiungi direzione se specificata
            var direction = parts.Length > 1 && parts[1].ToLower() == "desc" ? " desc" : " asc";
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
