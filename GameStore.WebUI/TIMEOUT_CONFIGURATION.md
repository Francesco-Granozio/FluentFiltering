# Configurazione Timeout per GameStore AI

## Problema Risolto
Il sistema ora gestisce correttamente i timeout per operazioni AI lunghe che richiedono più di 100 secondi.

## ⚠️ **PROBLEMA SPECIFICO RISOLTO**
**Errore:** `System.Threading.Tasks.TaskCanceledException: The request was canceled due to the configured HttpClient.Timeout of 100 seconds elapsing.`

**Causa:** OllamaSharp crea internamente un HttpClient con timeout predefinito di 100 secondi che sovrascriveva le nostre configurazioni.

## Configurazioni Applicate

### 1. Blazor Server SignalR Timeout
**File:** `Program.cs`
- `JSInteropDefaultCallTimeout`: 10 minuti (600 secondi)
- `ClientTimeoutInterval`: 10 minuti per SignalR Hub
- `HandshakeTimeout`: 2 minuti per handshake iniziale
- `KeepAliveInterval`: 15 secondi per mantenere connessione attiva

### 2. HttpClient Timeout
**File:** `Program.cs`
- Timeout per client HTTP "Ollama": 15 minuti
- Gestisce le chiamate HTTP verso il servizio Ollama

### 3. Ollama API Timeout
**File:** `appsettings.json` e `appsettings.Development.json`
- `RequestTimeout`: 15 minuti (produzione) / 20 minuti (sviluppo)
- `ConnectionTimeout`: 2 minuti (produzione) / 5 minuti (sviluppo)

### 4. Database Timeout
**File:** `Program.cs`
- `CommandTimeout`: 30 secondi per query SQL
- `maxRetryDelay`: 30 secondi per retry automatici

## Come Funziona

### Timeout Gestiti Automaticamente
1. **Blazor Server**: Non si disconnette più dopo 100 secondi
2. **SignalR**: Mantiene la connessione attiva per operazioni lunghe
3. **Ollama API**: Timeout configurabili per richieste AI
4. **HttpClient**: Timeout esteso per chiamate esterne
5. **CancellationTokenSource**: Gestisce timeout personalizzati bypassando HttpClient

### Gestione Errori
- Se l'AI impiega troppo tempo, ricevi un messaggio informativo
- Il sistema non si blocca più indefinitamente
- I timeout sono configurabili per ambiente (sviluppo vs produzione)
- **Task.Run** evita il timeout HttpClient predefinito di OllamaSharp

### Soluzione Tecnica Implementata
```csharp
// Nel ChatService.cs
using CancellationTokenSource cts = new(_requestTimeout);

await Task.Run(async () =>
{
    await foreach (var stream in _ollamaClient.GenerateAsync(prompt).WithCancellation(cts.Token))
    {
        // Elabora stream...
    }
}, cts.Token);
```

## Personalizzazione

### Per Aumentare ulteriormente i Timeout:
Modifica i valori in `appsettings.Development.json`:

```json
{
  "Ollama": {
    "RequestTimeout": "00:30:00",    // 30 minuti
    "ConnectionTimeout": "00:10:00"  // 10 minuti
  }
}
```

### Per Ridurre i Timeout (prestazioni):
```json
{
  "Ollama": {
    "RequestTimeout": "00:05:00",    // 5 minuti
    "ConnectionTimeout": "00:01:00"  // 1 minuto
  }
}
```

## Note Tecniche

- I timeout sono gestiti tramite `CancellationTokenSource`
- Il sistema continua a funzionare anche se un'operazione va in timeout
- I log mostrano quando si verificano timeout per debugging
- Le configurazioni sono caricate automaticamente all'avvio

## Troubleshooting

Se continui a riscontrare timeout:
1. Verifica che Ollama sia in esecuzione
2. Controlla i log per messaggi di timeout
3. Aumenta i valori di timeout se necessario
4. Considera di ottimizzare le richieste AI (prompt più corti, ecc.)

## Ambiente di Sviluppo vs Produzione

- **Sviluppo**: Timeout più lunghi per debugging
- **Produzione**: Timeout bilanciati per UX ottimale
- **Logging**: Più dettagliato in sviluppo per troubleshooting
