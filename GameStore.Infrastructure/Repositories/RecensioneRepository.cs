using GameStore.Domain.Entities;

namespace GameStore.Infrastructure.Repositories;

/// <summary>
/// Implementazione del repository per Recensioni
/// </summary>
public class RecensioneRepository : RepositoryGenerico<Recensione>, IRecensioneRepository
{
    public RecensioneRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Recensione>> GetByUtenteIdAsync(Guid utenteId, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        IQueryable<Recensione> query = _dbSet.AsQueryable();

        if (includeDeleted)
        {
            query = query.IgnoreQueryFilters();
        }

        return await query
            .Where(r => r.UtenteId == utenteId)
            .Include(r => r.Utente)
            .Include(r => r.Gioco)
            .Include(r => r.Acquisto)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Recensione>> GetByGiocoIdAsync(Guid giocoId, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        IQueryable<Recensione> query = _dbSet.AsQueryable();

        if (includeDeleted)
        {
            query = query.IgnoreQueryFilters();
        }

        return await query
            .Where(r => r.GiocoId == giocoId)
            .Include(r => r.Utente)
            .Include(r => r.Gioco)
            .Include(r => r.Acquisto)
            .ToListAsync(cancellationToken);
    }

    public async Task<Recensione?> GetByUtenteEGiocoAsync(Guid utenteId, Guid giocoId, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        IQueryable<Recensione> query = _dbSet.AsQueryable();

        if (includeDeleted)
        {
            query = query.IgnoreQueryFilters();
        }

        return await query
            .Where(r => r.UtenteId == utenteId && r.GiocoId == giocoId)
            .Include(r => r.Utente)
            .Include(r => r.Gioco)
            .Include(r => r.Acquisto)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Recensione>> GetRecensioniVerificateAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        IQueryable<Recensione> query = _dbSet.AsQueryable();

        if (includeDeleted)
        {
            query = query.IgnoreQueryFilters();
        }

        return await query
            .Where(r => r.IsRecensioneVerificata)
            .Include(r => r.Utente)
            .Include(r => r.Gioco)
            .Include(r => r.Acquisto)
            .ToListAsync(cancellationToken);
    }

    public async Task<double?> GetMediaPunteggiAsync(Guid giocoId, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        IQueryable<Recensione> query = _dbSet.AsQueryable();

        if (includeDeleted)
        {
            query = query.IgnoreQueryFilters();
        }

        return await query
            .Where(r => r.GiocoId == giocoId)
            .AverageAsync(r => (double?)r.Punteggio, cancellationToken);
    }
}
