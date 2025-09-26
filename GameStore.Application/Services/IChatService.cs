namespace GameStore.Application.Services;

/// <summary>
/// Servizio per la chat AI con Ollama
/// </summary>
public interface IChatService
{
    /// <summary>
    /// Invia un messaggio all'AI e restituisce la risposta
    /// </summary>
    /// <param name="message">Messaggio dell'utente</param>
    /// <param name="context">Contesto dei dati (opzionale)</param>
    /// <returns>Risposta dell'AI</returns>
    Task<string> SendMessageAsync(string message, object? context = null);

    /// <summary>
    /// Invia un messaggio all'AI e restituisce la risposta come stream
    /// </summary>
    /// <param name="message">Messaggio dell'utente</param>
    /// <param name="context">Contesto dei dati (opzionale)</param>
    /// <returns>Stream della risposta dell'AI</returns>
    IAsyncEnumerable<string> SendMessageStreamAsync(string message, object? context = null);

    /// <summary>
    /// Verifica se il servizio è disponibile
    /// </summary>
    /// <returns>True se il servizio è disponibile</returns>
    Task<bool> IsAvailableAsync();

    /// <summary>
    /// Ottiene il nome del modello AI attualmente configurato
    /// </summary>
    /// <returns>Nome del modello</returns>
    string GetModelName();
}

