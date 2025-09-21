using GameStore.Domain.Interfaces;
using GameStore.Shared.DTOs;
using GameStore.Shared.DTOs.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace GameStore.Infrastructure.Repositories;

/// <summary>
/// Implementazione del repository per i giochi acquistati
/// </summary>
public class GiochiAcquistatiRepository : IGiochiAcquistatiRepository
{
    private readonly ApplicationDbContext _context;

    public GiochiAcquistatiRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Ottiene i giochi acquistati con informazioni dell'utente e del gioco (paginati)
    /// </summary>
    public async Task<Shared.DTOs.Common.PagedResult<GiochiAcquistatiDto>> GetGiochiAcquistatiAsync(
        FilterRequest request, 
        CancellationToken cancellationToken = default)
    {
        var query = BuildBaseQuery();

        // Applica filtro dinamico se presente
        query = ApplyFilter(query, request.Filter);

        // Applica ordinamento
        query = ApplyOrdering(query, request.OrderBy);

        // Conta il totale
        var totalItems = await query.CountAsync(cancellationToken);

        // Applica paginazione
        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new Shared.DTOs.Common.PagedResult<GiochiAcquistatiDto>
        {
            Items = items,
            TotalItems = totalItems,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    /// <summary>
    /// Ottiene tutti i giochi acquistati
    /// </summary>
    public async Task<IEnumerable<GiochiAcquistatiDto>> GetAllGiochiAcquistatiAsync(
        CancellationToken cancellationToken = default)
    {
        var query = BuildBaseQuery();
        query = ApplyOrdering(query, null);

        return await query.ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Ottiene i giochi acquistati con filtro personalizzato
    /// </summary>
    public async Task<IEnumerable<GiochiAcquistatiDto>> GetGiochiAcquistatiAsync(
        string? filter = null,
        string? orderBy = null,
        CancellationToken cancellationToken = default)
    {
        var query = BuildBaseQuery();
        query = ApplyFilter(query, filter);
        query = ApplyOrdering(query, orderBy);

        return await query.ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Costruisce la query base per i giochi acquistati
    /// </summary>
    private IQueryable<GiochiAcquistatiDto> BuildBaseQuery()
    {
        return from utente in _context.Utenti
               join acquisto in _context.Acquisti on utente.Id equals acquisto.UtenteId
               join gioco in _context.Giochi on acquisto.GiocoId equals gioco.Id
               where !utente.IsCancellato && !acquisto.IsCancellato && !gioco.IsCancellato
               select new GiochiAcquistatiDto
               {
                   AcquistoId = acquisto.Id,
                   UtenteId = utente.Id,
                   UtenteUsername = utente.Username,
                   UtenteEmail = utente.Email,
                   UtenteNomeCompleto = utente.NomeCompleto,
                   DataAcquisto = acquisto.DataAcquisto,
                   PrezzoPagato = acquisto.PrezzoPagato,
                   Quantita = acquisto.Quantita,
                   MetodoPagamento = acquisto.MetodoPagamento,
                   CodiceSconto = acquisto.CodiceSconto ?? string.Empty,
                   GiocoId = gioco.Id,
                   GiocoTitolo = gioco.Titolo,
                   GiocoDescrizione = gioco.Descrizione,
                   GiocoPrezzoListino = gioco.PrezzoListino,
                   GiocoDataRilascio = gioco.DataRilascio,
                   GiocoGenere = gioco.Genere,
                   GiocoPiattaforma = gioco.Piattaforma,
                   GiocoSviluppatore = gioco.Sviluppatore,
                   DataCreazione = acquisto.DataCreazione
               };
    }

    /// <summary>
    /// Applica il filtro alla query
    /// </summary>
    private IQueryable<GiochiAcquistatiDto> ApplyFilter(IQueryable<GiochiAcquistatiDto> query, string? filter)
    {
        if (string.IsNullOrEmpty(filter))
            return query;

        return query.Where(x => 
            x.UtenteUsername.Contains(filter) ||
            x.UtenteEmail.Contains(filter) ||
            x.UtenteNomeCompleto.Contains(filter) ||
            x.MetodoPagamento.Contains(filter) ||
            x.CodiceSconto.Contains(filter) ||
            x.GiocoTitolo.Contains(filter) ||
            x.GiocoDescrizione.Contains(filter) ||
            x.GiocoGenere.Contains(filter) ||
            x.GiocoPiattaforma.Contains(filter) ||
            x.GiocoSviluppatore.Contains(filter));
    }

    /// <summary>
    /// Applica l'ordinamento alla query
    /// </summary>
    private IQueryable<GiochiAcquistatiDto> ApplyOrdering(IQueryable<GiochiAcquistatiDto> query, string? orderBy)
    {
        if (!string.IsNullOrEmpty(orderBy))
        {
            return query.OrderBy(orderBy);
        }

        return query.OrderByDescending(x => x.DataAcquisto);
    }
}
