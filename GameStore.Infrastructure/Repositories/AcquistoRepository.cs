using GameStore.Domain.Interfaces;
using GameStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Infrastructure.Repositories;

/// <summary>
/// Implementazione del repository per Acquisti
/// </summary>
public class AcquistoRepository : RepositoryGenerico<Acquisto>, IAcquistoRepository
{
    public AcquistoRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Acquisto>> GetByUtenteIdAsync(Guid utenteId, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();
        
        if (includeDeleted)
        {
            query = query.IgnoreQueryFilters();
        }

        return await query
            .Where(a => a.UtenteId == utenteId)
            .Include(a => a.Utente)
            .Include(a => a.Gioco)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Acquisto>> GetByGiocoIdAsync(Guid giocoId, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();
        
        if (includeDeleted)
        {
            query = query.IgnoreQueryFilters();
        }

        return await query
            .Where(a => a.GiocoId == giocoId)
            .Include(a => a.Utente)
            .Include(a => a.Gioco)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasUtenteAcquistatoGiocoAsync(Guid utenteId, Guid giocoId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(a => a.UtenteId == utenteId && a.GiocoId == giocoId, cancellationToken);
    }

    public async Task<IEnumerable<Acquisto>> GetByPeriodoAsync(DateTime dataInizio, DateTime dataFine, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();
        
        if (includeDeleted)
        {
            query = query.IgnoreQueryFilters();
        }

        return await query
            .Where(a => a.DataAcquisto >= dataInizio && a.DataAcquisto <= dataFine)
            .Include(a => a.Utente)
            .Include(a => a.Gioco)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid id, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        return await ExistsAsync(a => a.Id == id, includeDeleted, cancellationToken);
    }
}
