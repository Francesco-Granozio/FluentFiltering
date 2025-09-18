using GameStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameStore.Infrastructure.Seeding;

/// <summary>
/// Implementazione del seeder per popolare il database con dati di test
/// Genera un grande volume di dati realistici per test e demo
/// </summary>
public class DatabaseSeeder : IDataSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DatabaseSeeder> _logger;
    private readonly Random _random = new();

    // Configurazione quantità dati
    private const int NUMERO_UTENTI = 1000;
    private const int NUMERO_GIOCHI = 200;
    private const int NUMERO_ACQUISTI_PER_UTENTE = 5; // In media
    private const int NUMERO_RECENSIONI_PER_UTENTE = 3; // In media

    public DatabaseSeeder(ApplicationDbContext context, ILogger<DatabaseSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> IsDatabaseSeededAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Utenti.AnyAsync(cancellationToken);
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Inizio seeding del database...");

        try
        {
            // Verifica se il database è già popolato
            if (await IsDatabaseSeededAsync(cancellationToken))
            {
                _logger.LogInformation("Database già popolato, seeding saltato.");
                return;
            }

            // Genera i dati in ordine per rispettare le foreign key
            var utenti = await GeneraUtenti(cancellationToken);
            var giochi = await GeneraGiochi(cancellationToken);
            var acquisti = await GeneraAcquisti(utenti, giochi, cancellationToken);
            await GeneraRecensioni(utenti, giochi, acquisti, cancellationToken);

            _logger.LogInformation($"Seeding completato: {utenti.Count} utenti, {giochi.Count} giochi, {acquisti.Count} acquisti, e recensioni generate.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il seeding del database");
            throw;
        }
    }

    private async Task<List<Utente>> GeneraUtenti(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Generazione di {NUMERO_UTENTI} utenti...");

        var utenti = new List<Utente>();
        var usernamesUsati = new HashSet<string>();
        var emailsUsate = new HashSet<string>();

        for (int i = 0; i < NUMERO_UTENTI; i++)
        {
            var utente = new Utente
            {
                Id = Guid.NewGuid(),
                Username = GeneraUsernameUnico(usernamesUsati),
                Email = GeneraEmailUnica(emailsUsate),
                NomeCompleto = $"{SeedData.Nomi[_random.Next(SeedData.Nomi.Length)]} {SeedData.Cognomi[_random.Next(SeedData.Cognomi.Length)]}",
                Paese = _random.NextDouble() < 0.8 ? SeedData.Paesi[_random.Next(SeedData.Paesi.Length)] : null,
                DataRegistrazione = GeneraDataRegistrazione()
            };

            utenti.Add(utente);
        }

        _context.Utenti.AddRange(utenti);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation($"{utenti.Count} utenti creati con successo.");

        return utenti;
    }

    private async Task<List<Gioco>> GeneraGiochi(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Generazione di {NUMERO_GIOCHI} giochi...");

        var giochi = new List<Gioco>();

        for (int i = 0; i < NUMERO_GIOCHI; i++)
        {
            var gioco = new Gioco
            {
                Id = Guid.NewGuid(),
                Titolo = SeedData.TitoliGiochi[_random.Next(SeedData.TitoliGiochi.Length)],
                Descrizione = SeedData.DescrizioniGiochi[_random.Next(SeedData.DescrizioniGiochi.Length)],
                PrezzoListino = GeneraPrezzo(),
                DataRilascio = _random.NextDouble() < 0.9 ? GeneraDataRilascio() : null,
                Genere = SeedData.Generi[_random.Next(SeedData.Generi.Length)],
                Piattaforma = SeedData.Piattaforme[_random.Next(SeedData.Piattaforme.Length)],
                Sviluppatore = SeedData.Sviluppatori[_random.Next(SeedData.Sviluppatori.Length)]
            };

            giochi.Add(gioco);
        }

        _context.Giochi.AddRange(giochi);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation($"{giochi.Count} giochi creati con successo.");

        return giochi;
    }

    private async Task<List<Acquisto>> GeneraAcquisti(List<Utente> utenti, List<Gioco> giochi, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Generazione di acquisti per {utenti.Count} utenti...");

        var acquisti = new List<Acquisto>();
        var acquistiPerUtente = new Dictionary<Guid, List<Guid>>(); // Utente -> Lista di giochi acquistati

        foreach (var utente in utenti)
        {
            acquistiPerUtente[utente.Id] = new List<Guid>();
            
            // Numero casuale di acquisti per utente (tra 1 e 10)
            var numeroAcquisti = _random.Next(1, 11);
            
            for (int i = 0; i < numeroAcquisti; i++)
            {
                var gioco = giochi[_random.Next(giochi.Count)];
                
                // Evita acquisti duplicati dello stesso gioco
                if (acquistiPerUtente[utente.Id].Contains(gioco.Id))
                    continue;

                var acquisto = new Acquisto
                {
                    Id = Guid.NewGuid(),
                    UtenteId = utente.Id,
                    GiocoId = gioco.Id,
                    DataAcquisto = GeneraDataAcquisto(utente.DataRegistrazione),
                    PrezzoPagato = GeneraPrezzoPagato(gioco.PrezzoListino),
                    Quantita = _random.Next(1, 4), // 1-3 copie
                    MetodoPagamento = SeedData.MetodiPagamento[_random.Next(SeedData.MetodiPagamento.Length)],
                    CodiceSconto = _random.NextDouble() < 0.3 ? SeedData.CodiciSconto[_random.Next(SeedData.CodiciSconto.Length)] : null
                };

                acquisti.Add(acquisto);
                acquistiPerUtente[utente.Id].Add(gioco.Id);
            }
        }

        _context.Acquisti.AddRange(acquisti);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation($"{acquisti.Count} acquisti creati con successo.");

        return acquisti;
    }

    private async Task GeneraRecensioni(List<Utente> utenti, List<Gioco> giochi, List<Acquisto> acquisti, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Generazione di recensioni...");

        var recensioni = new List<Recensione>();
        var recensioniPerUtenteGioco = new Dictionary<string, bool>(); // "UtenteId-GiocoId" -> bool

        foreach (var utente in utenti)
        {
            // Numero casuale di recensioni per utente (tra 0 e 8)
            var numeroRecensioni = _random.Next(0, 9);
            
            for (int i = 0; i < numeroRecensioni; i++)
            {
                var gioco = giochi[_random.Next(giochi.Count)];
                var key = $"{utente.Id}-{gioco.Id}";
                
                // Evita recensioni multiple dello stesso utente per lo stesso gioco
                if (recensioniPerUtenteGioco.ContainsKey(key))
                    continue;

                // Verifica se l'utente ha acquistato il gioco
                var acquistoVerificato = acquisti.FirstOrDefault(a => a.UtenteId == utente.Id && a.GiocoId == gioco.Id);
                var isVerificata = acquistoVerificato != null;

                var recensione = new Recensione
                {
                    Id = Guid.NewGuid(),
                    UtenteId = utente.Id,
                    GiocoId = gioco.Id,
                    Punteggio = _random.Next(1, 6), // 1-5 stelle
                    Titolo = SeedData.TitoliRecensioni[_random.Next(SeedData.TitoliRecensioni.Length)],
                    Corpo = SeedData.CorpiRecensioni[_random.Next(SeedData.CorpiRecensioni.Length)],
                    DataRecensione = GeneraDataRecensione(utente.DataRegistrazione),
                    IsRecensioneVerificata = isVerificata,
                    AcquistoId = acquistoVerificato?.Id
                };

                recensioni.Add(recensione);
                recensioniPerUtenteGioco[key] = true;
            }
        }

        _context.Recensioni.AddRange(recensioni);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation($"{recensioni.Count} recensioni create con successo.");
    }

    #region Metodi Helper per Generazione Dati

    private string GeneraUsernameUnico(HashSet<string> usernamesUsati)
    {
        string username;
        do
        {
            var nome = SeedData.Nomi[_random.Next(SeedData.Nomi.Length)];
            var numero = _random.Next(1, 1000);
            username = $"{nome.ToLower()}{numero}";
        } while (usernamesUsati.Contains(username));

        usernamesUsati.Add(username);
        return username;
    }

    private string GeneraEmailUnica(HashSet<string> emailsUsate)
    {
        string email;
        do
        {
            var nome = SeedData.Nomi[_random.Next(SeedData.Nomi.Length)].ToLower();
            var cognome = SeedData.Cognomi[_random.Next(SeedData.Cognomi.Length)].ToLower();
            var dominio = SeedData.DominiEmail[_random.Next(SeedData.DominiEmail.Length)];
            var numero = _random.Next(1, 1000);
            email = $"{nome}.{cognome}{numero}@{dominio}";
        } while (emailsUsate.Contains(email));

        emailsUsate.Add(email);
        return email;
    }

    private DateTime GeneraDataRegistrazione()
    {
        var giorniPassati = _random.Next(1, 365 * 3); // Ultimi 3 anni
        return DateTime.UtcNow.AddDays(-giorniPassati);
    }

    private DateTime GeneraDataRilascio()
    {
        var giorniPassati = _random.Next(1, 365 * 5); // Ultimi 5 anni
        return DateTime.UtcNow.AddDays(-giorniPassati);
    }

    private DateTime GeneraDataAcquisto(DateTime dataRegistrazione)
    {
        var giorniDopoRegistrazione = _random.Next(1, (int)(DateTime.UtcNow - dataRegistrazione).TotalDays);
        return dataRegistrazione.AddDays(giorniDopoRegistrazione);
    }

    private DateTime GeneraDataRecensione(DateTime dataRegistrazione)
    {
        var giorniDopoRegistrazione = _random.Next(7, (int)(DateTime.UtcNow - dataRegistrazione).TotalDays);
        return dataRegistrazione.AddDays(giorniDopoRegistrazione);
    }

    private decimal GeneraPrezzo()
    {
        var prezzi = new[] { 9.99m, 19.99m, 29.99m, 39.99m, 49.99m, 59.99m, 69.99m, 79.99m };
        return prezzi[_random.Next(prezzi.Length)];
    }

    private decimal GeneraPrezzoPagato(decimal prezzoListino)
    {
        // 70% paga il prezzo pieno, 30% con sconto
        if (_random.NextDouble() < 0.7)
            return prezzoListino;

        // Sconto tra 10% e 50%
        var scontoPercentuale = _random.Next(10, 51);
        return prezzoListino * (100 - scontoPercentuale) / 100;
    }

    #endregion
}
