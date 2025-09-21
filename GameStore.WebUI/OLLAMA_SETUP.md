# 🦙 Configurazione Ollama per GameStore AI

## Prerequisiti

1. **Installa Ollama** dal sito ufficiale: https://ollama.ai/
2. **Avvia il servizio Ollama** sul tuo sistema

## Installazione del Modello

### 1. Scarica il modello gpt-oss:20b

```bash
ollama pull gpt-oss:20b
```

### 2. Verifica l'installazione

```bash
ollama list
```

Dovresti vedere `gpt-oss:20b` nella lista dei modelli installati.

## Configurazione dell'Applicazione

### 1. Verifica la configurazione in appsettings.json

```json
{
  "Ollama": {
    "Uri": "http://localhost:11434",
    "Model": "gpt-oss:20b"
  }
}
```

### 2. Avvia l'applicazione

```bash
dotnet run
```

## Test del Servizio

1. Vai alla pagina "Giochi Acquistati"
2. Clicca "Apri Chat"
3. Verifica che il badge mostri "Online" (verde)
4. Invia un messaggio per testare l'AI

## Risoluzione Problemi

### Il servizio mostra "Offline"

1. **Verifica che Ollama sia in esecuzione:**
   ```bash
   ollama serve
   ```

2. **Verifica che il modello sia installato:**
   ```bash
   ollama list | grep gpt-oss
   ```

3. **Testa la connessione:**
   ```bash
   curl http://localhost:11434/api/tags
   ```

### Errore di connessione

- Assicurati che Ollama sia in ascolto su `localhost:11434`
- Verifica che non ci siano firewall che bloccano la connessione
- Controlla i log dell'applicazione per errori dettagliati

### Modello non trovato

Se `gpt-oss:20b` non è disponibile, puoi:

1. **Usare un modello alternativo:**
   ```bash
   ollama pull llama3.1:8b
   ```
   
   E aggiornare `appsettings.json`:
   ```json
   {
     "Ollama": {
       "Model": "llama3.1:8b"
     }
   }
   ```

2. **Verificare modelli disponibili:**
   ```bash
   ollama list
   ```

## Funzionalità AI

L'AI può rispondere a domande come:

- "Qual è il gioco più venduto?"
- "Quanto abbiamo guadagnato questo mese?"
- "Quali sono i generi più popolari?"
- "Mostrami statistiche sui metodi di pagamento"
- "Chi sono i nostri clienti più attivi?"

## Note Tecniche

- **Modello**: gpt-oss:20b (20 miliardi di parametri)
- **RAM richiesta**: ~12-16GB per il modello completo
- **Porta**: 11434 (default Ollama)
- **Protocollo**: HTTP REST API

## Supporto

Per problemi con Ollama, consulta la documentazione ufficiale: https://github.com/ollama/ollama

