# 🎮 GameStore - FluentFiltering Demo

Una **webapp dimostrativa** per testare il **filtraggio intelligente** tramite **AI e Ollama**, sviluppata in ASP.NET Core con Clean Architecture.

## 📖 Descrizione

**GameStore FluentFiltering** è un'applicazione demo che simula la gestione di un negozio di videogiochi, con focus particolare sul **filtraggio intelligente dei dati** attraverso l'intelligenza artificiale. 

L'utente può interrogare i dati usando **linguaggio naturale** in italiano, e l'AI (tramite Ollama) converte automaticamente le richieste in filtri strutturati per la griglia dati.

### 🔥 Funzionalità Principali

- **🤖 Filtraggio AI**: Converti richieste in linguaggio naturale in filtri avanzati
- **📊 Dashboard interattivo**: Gestione completa di giochi, utenti, acquisti e recensioni  
- **🎯 Chat intelligente**: Assistente AI per analisi dei dati e statistiche
- **📋 Griglia dati avanzata**: Filtraggio, ordinamento e impaginazione con Radzen
- **🔄 Clean Architecture**: Separazione dei layer e responsabilità
- **🌐 UI moderna**: Interfaccia responsive sviluppata con Blazor Server

## 🚀 Demo del Filtraggio Intelligente

### Esempi di Richieste AI

Puoi interrogare i dati usando frasi naturali come:

```
"Mostrami i giochi acquistati da utenti con email gmail"
"Trova tutti gli acquisti di gennaio 2024"
"Filtra i giochi di genere RPG costati più di 50 euro"
"Mostra gli acquisti con sconto maggiore del 10%"
"Cerca i giochi sviluppati da CD Projekt RED"
```

### Come Funziona

1. **Input utente**: Scrivi la tua richiesta in linguaggio naturale
2. **Elaborazione AI**: Ollama converte la richiesta in filtri JSON strutturati
3. **Applicazione filtri**: Il sistema applica automaticamente i filtri alla griglia dati
4. **Risultati**: Visualizzazione immediata dei dati filtrati

## 🛠️ Tecnologie Utilizzate

- **Backend**: ASP.NET Core 9.0
- **Frontend**: Blazor Server
- **Database**: Entity Framework Core con SQL Server LocalDB
- **AI/ML**: Ollama con modelli LLM (gpt-oss:20b, llama3.1:8b)
- **UI Components**: Radzen DataGrid
- **Architettura**: Clean Architecture (Domain, Application, Infrastructure, WebUI)

## 🏗️ Architettura del Progetto

```
GameStore/
├── 📁 GameStore.Domain/         # Entità di dominio e interfaces
├── 📁 GameStore.Application/    # Logica di business e servizi
├── 📁 GameStore.Infrastructure/ # Accesso ai dati e implementazioni
├── 📁 GameStore.WebUI/         # Interfaccia utente Blazor
├── 📁 GameStore.Shared/        # DTOs e modelli condivisi
└── 📁 GameStore.Mapping/       # Mappatura oggetti AutoMapper
```

### 🎯 Componenti Chiave

- **`ChatService`**: Integrazione con Ollama per elaborazione AI
- **`LinqFilterBuilder`**: Costruzione dinamica di espressioni LINQ
- **`FilterModels`**: Modelli per filtri strutturati JSON
- **`DatabaseSeeder`**: Popolamento database con dati demo

## 📋 Prerequisiti

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Ollama](https://ollama.ai/) installato e configurato
- SQL Server LocalDB
## 🚀 Installazione e Configurazione

### 1. 📥 Clona il Repository

```bash
git clone <repository-url>
cd FluentFiltering
```

### 2. 🦙 Configura Ollama

```bash
# Installa Ollama dal sito ufficiale: https://ollama.ai/

# Scarica il modello AI (richiede ~16GB RAM)
ollama pull gpt-oss:20b

# Oppure usa un modello più leggero
ollama pull llama3.1:8b

# Avvia il servizio
ollama serve
```

### 3. ⚙️ Configura l'Applicazione

Modifica `appsettings.json` se necessario:

```json
{
  "Ollama": {
    "Uri": "http://localhost:11434",
    "Model": "gpt-oss:20b",
    "RequestTimeout": "00:15:00",
    "ConnectionTimeout": "00:02:00"
  }
}
```

### 4. 🗃️ Configura il Database

```bash
# Ripristina i pacchetti
dotnet restore

# Applica le migrazioni
dotnet ef database update --project GameStore.Infrastructure --startup-project GameStore.WebUI

# Il database verrà automaticamente popolato con dati demo al primo avvio
```

### 5. ▶️ Avvia l'Applicazione

```bash
dotnet run --project GameStore.WebUI
```

L'applicazione sarà disponibile su `https://localhost:7018`

## 📱 Come Usare la Demo

### 1. 🏠 Homepage
- Visualizza statistiche generali del negozio
- Naviga tra le diverse sezioni

### 2. 🎮 Gestione Giochi
- Visualizza, crea, modifica ed elimina giochi
- Filtraggio avanzato per genere, piattaforma, sviluppatore

### 3. 👥 Gestione Utenti
- Gestione anagrafica clienti
- Storico acquisti per utente

### 4. 🛒 Acquisti e Recensioni
- Visualizzazione acquisti con dettagli completi
- Sistema di recensioni e valutazioni

### 5. 🤖 **Chat AI (Funzionalità Principale)**
- Vai su "Giochi Acquistati"
- Clicca "Apri Chat"
- Verifica che lo status sia "Online" (verde)
- Scrivi richieste in linguaggio naturale per filtrare i dati

### Esempi Pratici di Utilizzo

```
Utente: "Mostrami tutti i giochi RPG acquistati nel 2024"
AI: [Applica filtri: Genere=RPG, DataAcquisto>=2024-01-01]

Utente: "Trova acquisti con sconto superiore al 15%"  
AI: [Applica filtri: CodiceSconto contiene caratteri, calcola sconto]

Utente: "Giochi di Cyberpunk acquistati da utenti Gmail"
AI: [Applica filtri: GiocoTitolo contiene "Cyberpunk", UtenteEmail contiene "gmail"]
```

## 🔧 Debug e Troubleshooting

### 🐛 Modalità Debug AI
Quando esegui l'app in debug, vedrai informazioni dettagliate su:
- Configurazione del servizio Ollama
- Modelli disponibili
- Prompt completi inviati all'AI
- Risposte token per token
- Statistiche di performance

### ❌ Problemi Comuni

**🔴 Status "Offline" nella chat**
```bash
# Verifica che Ollama sia attivo
ollama serve

# Controlla modelli installati
ollama list

# Testa la connessione
curl http://localhost:11434/api/tags
```

**⚠️ Timeout delle richieste**
- Il modello potrebbe richiedere più RAM
- Prova con `llama3.1:8b` (più leggero)
- Aumenta i timeout in `appsettings.json`

**🗄️ Errori di database**
```bash
# Ricrea il database
dotnet ef database drop --project GameStore.Infrastructure --startup-project GameStore.WebUI
dotnet ef database update --project GameStore.Infrastructure --startup-project GameStore.WebUI
```

## 🎯 Obiettivi Dimostrativi

Questa demo mostra come:

1. **🧠 AI Integration**: Integrare modelli LLM locali in applicazioni web
2. **🔄 Natural Language Processing**: Convertire linguaggio naturale in filtri strutturati  
3. **⚡ Real-time Filtering**: Applicazione dinamica di filtri complessi
4. **🏛️ Clean Architecture**: Separazione delle responsabilità e testabilità
5. **🎨 Modern UI**: Interfacce utente moderne e responsive

## 🤝 Contributi

Questa è una demo educativa. Sentiti libero di:
- Sperimentare con diversi modelli Ollama
- Estendere le funzionalità di filtraggio
- Migliorare l'interfaccia utente  
- Aggiungere nuovi tipi di analisi AI

## 📄 Licenza

Questo progetto è rilasciato sotto licenza MIT. Vedi il file `LICENSE.txt` per i dettagli.

---

**🎮 Happy Gaming & Happy Coding!** 

*Sviluppato per dimostrare le potenzialità del filtraggio intelligente con AI e Ollama*