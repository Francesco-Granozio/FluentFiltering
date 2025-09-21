# 🐛 Debug AI - Guida al Debug del Modello

## Modalità Debug Attiva

Quando l'applicazione è in esecuzione con il debugger attaccato (`System.Diagnostics.Debugger.IsAttached`), il servizio AI mostra informazioni dettagliate sul ragionamento del modello.

## Informazioni di Debug Disponibili

### 1. **Configurazione del Servizio**
```
=== DEBUG MODE: CONFIGURAZIONE CHAT SERVICE ===
Ollama URI: http://localhost:11434
Modello: gpt-oss:20b
=== FINE CONFIGURAZIONE ===
```

### 2. **Modelli Disponibili**
```
=== DEBUG MODE: MODELLI DISPONIBILI ===
- gpt-oss:20b (Size: 1234567890 bytes, Modified: 2024-01-15T10:30:00Z)
- llama3.1:8b (Size: 987654321 bytes, Modified: 2024-01-14T15:45:00Z)
Modello cercato: gpt-oss:20b
=== FINE MODELLI ===
```

### 3. **Prompt Completo**
```
=== DEBUG MODE: PROMPT COMPLETO ===
Sei l'assistente AI di GameStore, un sistema di gestione di un negozio di videogiochi.
Il tuo compito è aiutare gli utenti ad analizzare e comprendere i dati dei giochi acquistati.
Rispondi sempre in italiano in modo chiaro e professionale.

Contesto dei dati attuali:
Hai accesso a 150 record di giochi acquistati.
Ogni record contiene informazioni su:
- Utente che ha acquistato (username, email, nome completo)
- Dettagli dell'acquisto (data, prezzo pagato, quantità, metodo pagamento, codice sconto)
- Informazioni del gioco (titolo, descrizione, prezzo listino, data rilascio, genere, piattaforma, sviluppatore)

Statistiche rapide:
- Ricavi totali: €12,450.00
- Utenti unici: 45
- Giochi unici: 78

=== DEBUG: ESEMPI DI DATI ===
- Cyberpunk 2077 acquistato da mario.rossi per €59.99 il 15/01/2024
- The Witcher 3 acquistato da luigi.verdi per €29.99 il 14/01/2024
- FIFA 24 acquistato da giuseppe.neri per €69.99 il 13/01/2024
=== FINE ESEMPI ===

Messaggio dell'utente:
Qual è il gioco più venduto?
=== FINE PROMPT ===
```

### 4. **Risposta in Tempo Reale**
Durante la generazione, vedrai ogni token della risposta in tempo reale nella console:
```
Il gioco più venduto nel nostro negozio è Cyberpunk 2077, che è stato acquistato da 12 utenti diversi per un totale di €719.88 in ricavi...
```

### 5. **Statistiche della Risposta**
```
=== DEBUG: STATISTICHE RISPOSTA ===
Token ricevuti: 156
Durata: 3.45 secondi
Velocità: 45.2 token/sec
=== FINE STATISTICHE ===
```

### 6. **Risposta Completa**
```
=== DEBUG MODE: RISPOSTA COMPLETA ===
Il gioco più venduto nel nostro negozio è Cyberpunk 2077, che è stato acquistato da 12 utenti diversi per un totale di €719.88 in ricavi. Questo gioco di ruolo open-world sviluppato da CD Projekt RED ha dimostrato di essere molto popolare tra i nostri clienti, probabilmente grazie alla sua ambientazione futuristico-distopica e alle meccaniche di gioco innovative.

In seconda posizione troviamo The Witcher 3: Wild Hunt, acquistato da 10 utenti per €299.90 totali, mentre FIFA 24 completa il podio con 8 acquisti per €559.92.
=== FINE RISPOSTA ===
```

## Come Attivare il Debug

### 1. **Visual Studio**
- Avvia l'applicazione con F5 (Debug)
- Il debug sarà automaticamente attivo

### 2. **Visual Studio Code**
- Usa il debugger di .NET
- Imposta breakpoint o avvia con "Start Debugging"

### 3. **Terminale**
```bash
# Avvia con debugger
dotnet run --configuration Debug

# Oppure con logging verboso
dotnet run --verbosity detailed
```

## Logging Configurato

Il file `appsettings.Development.json` è configurato per mostrare log dettagliati:

```json
{
  "Logging": {
    "LogLevel": {
      "GameStore.Application.Services.ChatService": "Debug"
    }
  }
}
```

## Dove Vedere i Log

### 1. **Console dell'Applicazione**
- I log appaiono direttamente nella console
- Usa `Console.Write()` per output in tempo reale

### 2. **Output di Visual Studio**
- Vai su "View" → "Output"
- Seleziona "Show output from: Debug"

### 3. **File di Log**
- I log vengono salvati nei file di log standard di .NET
- Controlla la cartella `logs/` se configurata

## Troubleshooting Debug

### Debug Non Funziona
1. **Verifica che il debugger sia attaccato:**
   ```csharp
   if (System.Diagnostics.Debugger.IsAttached)
   {
       // Debug code here
   }
   ```

2. **Controlla i log:**
   - Assicurati che il livello di logging sia "Debug"
   - Verifica che il logger sia configurato correttamente

3. **Verifica la configurazione:**
   - Controlla `appsettings.Development.json`
   - Assicurati di essere in modalità Development

### Performance del Debug
- Il debug aggiunge overhead alle performance
- Usa solo in modalità Development
- Disabilita in produzione per evitare log eccessivi

## Esempi di Debug

### Analisi di una Query Complessa
```
Messaggio: "Mostrami i giochi più redditizi del mese scorso"
Prompt: [include context with monthly data]
Risposta: [detailed analysis with specific games and revenue]
Statistiche: 234 token in 4.2 secondi (55.7 token/sec)
```

### Debug di Errori
```
Errore: "Ollama service not available"
Debug: Mostra modelli disponibili vs modello cercato
Risoluzione: Verifica che gpt-oss:20b sia installato
```

## Note Importanti

- **Sicurezza**: I log di debug possono contenere dati sensibili
- **Performance**: Il debug rallenta l'esecuzione
- **Produzione**: Disabilita sempre il debug in produzione
- **Privacy**: Non loggare mai password o dati personali

## Comandi Utili

```bash
# Verifica modelli Ollama
ollama list

# Test connessione
curl http://localhost:11434/api/tags

# Log in tempo reale
dotnet run --verbosity detailed | grep "DEBUG"
```

