using GameStore.Shared.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OllamaSharp;
using OllamaSharp.Models;
using System.Text;

namespace GameStore.Application.Services;

/// <summary>
/// Implementazione del servizio chat con OllamaSharp
/// </summary>
public class ChatService : IChatService
{
    private readonly OllamaApiClient _ollamaClient;
    private readonly ILogger<ChatService> _logger;
    private readonly string _modelName;
    private readonly string _ollamaUri;
    private readonly TimeSpan _requestTimeout;
    private readonly TimeSpan _connectionTimeout;
    
    // Memoria di sessione (non persistente): conserva una breve cronologia dei messaggi
    private readonly List<(string Role, string Content)> _sessionHistory = new();

    // Prompt di sistema fisso per guidare il modello (Radzen filters JSON)
    private const string SystemPrompt =
        "Sei \"GameStore Grid Filter Assistant\", un assistente AI per GameStore. Il tuo unico obiettivo è TRADURRE il prompt dell’utente in un elenco di filtri per Radzen DataGrid riferiti ai campi di GiochiAcquistatiDto. Rispondi sempre e solo con JSON valido, senza markdown, senza testo extra.\n\n" +
        "DATI\n" +
        "- La griglia mostra elementi di List<GiochiAcquistatiDto> giochi\n" +
        "- Campi disponibili: AcquistoId (Guid), UtenteId (Guid), UtenteUsername (string), UtenteEmail (string), UtenteNomeCompleto (string), DataAcquisto (DateTime), PrezzoPagato (decimal), Quantita (int), MetodoPagamento (string?), CodiceSconto (string), GiocoId (Guid), GiocoTitolo (string), GiocoDescrizione (string?), GiocoPrezzoListino (decimal), GiocoDataRilascio (DateTime?), GiocoGenere (string?), GiocoPiattaforma (string?), GiocoSviluppatore (string?), DataCreazione (DateTime).\n\n" +
        "OUTPUT\n" +
        "- Produci un array JSON di filtri. Ogni filtro ha: Property (string), FilterOperator (string: Contains|DoesNotContain|StartsWith|EndsWith|Equals|NotEquals|LessThan|LessThanOrEquals|GreaterThan|GreaterThanOrEquals|In|NotIn|IsNull|IsNotNull|IsEmpty|IsNotEmpty|Between), FilterValue (string|number|date|null), FilterValue2 (opzionale per Between), Logical (opzionale: And|Or per combinare due filtri sulla stessa colonna).\n" +
        "- Esempio: [{\"Property\":\"GiocoTitolo\",\"FilterOperator\":\"Contains\",\"FilterValue\":\"Cuphead\"},{\"Property\":\"DataAcquisto\",\"FilterOperator\":\"GreaterThanOrEquals\",\"FilterValue\":\"2020-01-01\"}]\n" +
        "- Tipi: usa numeri per int/decimal (es. 19.99), stringhe per testo, ISO 8601 per date (es. 2023-01-31 o 2023-01-31T00:00:00Z). Evita conversioni ambigue.\n" +
        "- Per Between fornisci entrambi i valori. Per In fornisci CSV. Per combinare due condizioni sulla stessa colonna, usa due oggetti con stessa Property e Logical=And|Or.\n\n" +
        "MEMORIA DI SESSIONE\n" +
        "- Se l’utente chiede raffinamenti, restituisci l’elenco completo dei filtri aggiornato (non solo il delta).\n\n" +
        "REGOLE\n" +
        "- Niente markdown. Niente codice C#. Solo JSON array.\n";

    public ChatService(ILogger<ChatService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _ollamaUri = configuration["Ollama:Uri"] ?? "http://localhost:11434";
        _modelName = configuration["Ollama:Model"] ?? "gpt-oss:20b";

        // Configura timeout personalizzati
        string requestTimeoutStr = configuration["Ollama:RequestTimeout"] ?? "00:15:00";
        string connectionTimeoutStr = configuration["Ollama:ConnectionTimeout"] ?? "00:02:00";
        _requestTimeout = TimeSpan.Parse(requestTimeoutStr);
        _connectionTimeout = TimeSpan.Parse(connectionTimeoutStr);

        try
        {
            Uri uri = new(_ollamaUri);
            
            // Crea OllamaApiClient con URI
            _ollamaClient = new OllamaApiClient(uri);
            _ollamaClient.SelectedModel = _modelName;
            
            // Configura il timeout a livello di HttpClient globale
            // Questo dovrebbe essere gestito dalle configurazioni in Program.cs

            _logger.LogInformation($"ChatService initialized with model: {_modelName} at {_ollamaUri}");

            // Debug: mostra la configurazione
            if (System.Diagnostics.Debugger.IsAttached)
            {
                _logger.LogInformation("=== DEBUG MODE: CONFIGURAZIONE CHAT SERVICE ===");
                _logger.LogInformation($"Ollama URI: {_ollamaUri}");
                _logger.LogInformation($"Modello: {_modelName}");
                _logger.LogInformation($"Request Timeout: {_requestTimeout}");
                _logger.LogInformation($"Connection Timeout: {_connectionTimeout}");
                _logger.LogInformation("=== FINE CONFIGURAZIONE ===");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize OllamaApiClient");
            throw;
        }
    }

    /// <summary>
    /// Invia un messaggio all'AI e restituisce la risposta completa
    /// </summary>
    public async Task<string> SendMessageAsync(string message, object? context = null)
    {
        try
        {
            _logger.LogInformation($"Sending message to AI: {message[..Math.Min(100, message.Length)]}...");

            // Crea il prompt con il contesto
            string prompt = BuildPrompt(message, context);

            // Debug: mostra il prompt completo in modalità debug
            if (System.Diagnostics.Debugger.IsAttached)
            {
                _logger.LogInformation("=== DEBUG MODE: PROMPT COMPLETO ===");
                _logger.LogInformation(prompt);
                _logger.LogInformation("=== FINE PROMPT ===");
            }

            // Genera la risposta
            StringBuilder response = new();
            int tokenCount = 0;
            DateTime startTime = DateTime.Now;

            // Usa CancellationTokenSource per gestire il timeout
            using CancellationTokenSource cts = new(_requestTimeout);

            try
            {
                // Usa Task.Run per evitare il timeout HttpClient predefinito
                await Task.Run(async () =>
                {
                    await foreach (GenerateResponseStream? stream in _ollamaClient.GenerateAsync(prompt).WithCancellation(cts.Token))
                    {
                        string streamResponse = stream?.Response ?? string.Empty;
                        response.Append(streamResponse);
                        tokenCount++;

                        // Debug: mostra la risposta in tempo reale
                        if (System.Diagnostics.Debugger.IsAttached)
                        {
                            Console.Write(streamResponse);
                        }
                    }
                }, cts.Token);
            }
            catch (OperationCanceledException) when (cts.Token.IsCancellationRequested)
            {
                _logger.LogWarning($"Request timeout after {_requestTimeout.TotalMinutes} minutes");
                return "La richiesta ha impiegato troppo tempo per essere elaborata. Il modello sta ancora elaborando la risposta, ma il timeout è stato raggiunto. Riprova con una richiesta più semplice o attendi che il sistema sia meno carico.";
            }

            DateTime endTime = DateTime.Now;
            TimeSpan duration = endTime - startTime;

            // Debug: mostra statistiche della risposta
            if (System.Diagnostics.Debugger.IsAttached)
            {
                _logger.LogInformation($"=== DEBUG: STATISTICHE RISPOSTA ===");
                _logger.LogInformation($"Token ricevuti: {tokenCount}");
                _logger.LogInformation($"Durata: {duration.TotalSeconds:F2} secondi");
                _logger.LogInformation($"Velocità: {tokenCount / duration.TotalSeconds:F2} token/sec");
                _logger.LogInformation("=== FINE STATISTICHE ===");
            }

            string fullResponse = response.ToString();

            // Aggiorna memoria di sessione con la risposta del modello
            if (!string.IsNullOrWhiteSpace(fullResponse))
            {
                _sessionHistory.Add(("assistant", fullResponse));
                // Mantieni dimensione massima (es. 20 messaggi)
                if (_sessionHistory.Count > 20)
                {
                    int removeCount = _sessionHistory.Count - 20;
                    _sessionHistory.RemoveRange(0, removeCount);
                }
            }

            // Debug: mostra la risposta completa
            if (System.Diagnostics.Debugger.IsAttached)
            {
                _logger.LogInformation("=== DEBUG MODE: RISPOSTA COMPLETA ===");
                _logger.LogInformation(fullResponse);
                _logger.LogInformation("=== FINE RISPOSTA ===");
            }

            _logger.LogInformation($"AI response length: {fullResponse.Length} characters");

            return fullResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending message to AI");
            return "Mi dispiace, si è verificato un errore durante l'elaborazione della tua richiesta. Riprova più tardi.";
        }
    }

    /// <summary>
    /// Invia un messaggio all'AI e restituisce la risposta come stream
    /// </summary>
    public async IAsyncEnumerable<string> SendMessageStreamAsync(string message, object? context = null)
    {
        string prompt = BuildPrompt(message, context);

        _logger.LogInformation($"Streaming message to AI: {message[..Math.Min(100, message.Length)]}...");

        // Debug: mostra il prompt completo in modalità debug
        if (System.Diagnostics.Debugger.IsAttached)
        {
            _logger.LogInformation("=== DEBUG MODE: STREAMING PROMPT ===");
            _logger.LogInformation(prompt);
            _logger.LogInformation("=== FINE PROMPT ===");
        }

        // Usa CancellationTokenSource per gestire il timeout
        using CancellationTokenSource cts = new(_requestTimeout);

        // Usa Task.Run per evitare il timeout HttpClient predefinito
        await foreach (GenerateResponseStream? stream in _ollamaClient.GenerateAsync(prompt).WithCancellation(cts.Token))
        {
            string response = stream?.Response ?? string.Empty;
            if (!string.IsNullOrEmpty(response))
            {
                // Debug: mostra ogni token in tempo reale
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    Console.Write(response);
                }

                yield return response;
            }
        }

        // Se arriviamo qui senza timeout, il stream è completato normalmente
    }

    /// <summary>
    /// Verifica se il servizio è disponibile
    /// </summary>
    public async Task<bool> IsAvailableAsync()
    {
        try
        {
            // Prova a elencare i modelli per verificare la connessione
            IEnumerable<Model> models = await _ollamaClient.ListLocalModelsAsync();

            // Debug: mostra tutti i modelli disponibili
            if (System.Diagnostics.Debugger.IsAttached)
            {
                _logger.LogInformation("=== DEBUG MODE: MODELLI DISPONIBILI ===");
                foreach (Model model in models)
                {
                    _logger.LogInformation($"- {model.Name} (Size: {model.Size} bytes, Modified: {model.ModifiedAt})");
                }
                _logger.LogInformation($"Modello cercato: {_modelName}");
                _logger.LogInformation("=== FINE MODELLI ===");
            }

            // Verifica se il modello configurato è disponibile
            bool modelExists = models.Any(m => m.Name.Contains(_modelName));

            _logger.LogInformation($"Ollama service available: {modelExists}");
            return modelExists;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ollama service not available");
            return false;
        }
    }

    /// <summary>
    /// Ottiene il nome del modello AI attualmente configurato
    /// </summary>
    public string GetModelName()
    {
        return _modelName;
    }

    /// <summary>
    /// Costruisce il prompt con il contesto dei dati
    /// </summary>
    private string BuildPrompt(string message, object? context)
    {
        StringBuilder promptBuilder = new();

        // Prompt di sistema fisso
        promptBuilder.AppendLine(SystemPrompt);

        // Memoria di sessione: riproduci una traccia sintetica dei turni precedenti
        if (_sessionHistory.Count > 0)
        {
            promptBuilder.AppendLine("=== CONTESTO DI SESSIONE PRECEDENTE ===");
            // Include solo le ultime 6 voci (3 turni user/assistant) per contenere la lunghezza
            foreach (var (Role, Content) in _sessionHistory.TakeLast(6))
            {
                promptBuilder.AppendLine($"[{Role}]\n{Content}\n");
            }
            promptBuilder.AppendLine("=== FINE CONTESTO DI SESSIONE ===\n");
        }

        // Contesto dei dati se disponibile
        if (context != null)
        {
            promptBuilder.AppendLine("Contesto dei dati attuali:");

            if (context is List<GiochiAcquistatiDto> giochiAcquistati)
            {
                promptBuilder.AppendLine($"Hai accesso a {giochiAcquistati.Count} record di giochi acquistati.");
                promptBuilder.AppendLine("Ogni record contiene informazioni su:");
                promptBuilder.AppendLine("- Utente che ha acquistato (username, email, nome completo)");
                promptBuilder.AppendLine("- Dettagli dell'acquisto (data, prezzo pagato, quantità, metodo pagamento, codice sconto)");
                promptBuilder.AppendLine("- Informazioni del gioco (titolo, descrizione, prezzo listino, data rilascio, genere, piattaforma, sviluppatore)");
                promptBuilder.AppendLine();

                // Statistiche rapide
                decimal totalRevenue = giochiAcquistati.Sum(g => g.PrezzoPagato);
                int uniqueUsers = giochiAcquistati.Select(g => g.UtenteId).Distinct().Count();
                int uniqueGames = giochiAcquistati.Select(g => g.GiocoId).Distinct().Count();

                promptBuilder.AppendLine($"Statistiche rapide:");
                promptBuilder.AppendLine($"- Ricavi totali: €{totalRevenue:F2}");
                promptBuilder.AppendLine($"- Utenti unici: {uniqueUsers}");
                promptBuilder.AppendLine($"- Giochi unici: {uniqueGames}");
                promptBuilder.AppendLine();

                // Debug: mostra alcuni esempi di dati
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    promptBuilder.AppendLine("=== DEBUG: ESEMPI DI DATI ===");
                    List<GiochiAcquistatiDto> sampleData = giochiAcquistati.Take(3).ToList();
                    foreach (GiochiAcquistatiDto? item in sampleData)
                    {
                        promptBuilder.AppendLine($"- {item.GiocoTitolo} acquistato da {item.UtenteUsername} per €{item.PrezzoPagato:F2} il {item.DataAcquisto:dd/MM/yyyy}");
                    }
                    promptBuilder.AppendLine("=== FINE ESEMPI ===");
                    promptBuilder.AppendLine();
                }
            }
        }

        promptBuilder.AppendLine("Messaggio dell'utente:");
        promptBuilder.AppendLine(message);

        string finalPrompt = promptBuilder.ToString();

        // Debug: mostra la lunghezza del prompt
        if (System.Diagnostics.Debugger.IsAttached)
        {
            _logger.LogInformation($"Prompt costruito: {finalPrompt.Length} caratteri");
        }

        // Aggiorna memoria di sessione con l'ultimo turno utente
        _sessionHistory.Add(("user", message));

        return finalPrompt;
    }
}
