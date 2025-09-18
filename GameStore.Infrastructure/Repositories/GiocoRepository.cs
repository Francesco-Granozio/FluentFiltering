using GameStore.Domain.Interfaces;
using GameStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Infrastructure.Repositories;

/// <summary>
/// Implementazione del repository per Giochi
/// </summary>
public class GiocoRepository : RepositoryGenerico<Gioco>, IGiocoRepository
{
    public GiocoRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Gioco>> GetTopSellingAsync(int count = 10, CancellationToken cancellationToken = default)
    {
        return await _context.Acquisti
            .Where(a => !a.IsCancellato)
            .GroupBy(a => a.GiocoId)
            .Select(g => new { GiocoId = g.Key, SalesCount = g.Sum(a => a.Quantita) })
            .OrderByDescending(x => x.SalesCount)
            .Take(count)
            .Join(_context.Giochi, 
                  x => x.GiocoId, 
                  g => g.Id, 
                  (x, g) => g)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Gioco>> GetByGenereAsync(string genere, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(g => g.Genere == genere)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Gioco>> GetByPiattaformaAsync(string piattaforma, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(g => g.Piattaforma == piattaforma)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Gioco>> GetBySviluppatoreAsync(string sviluppatore, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(g => g.Sviluppatore == sviluppatore)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid id, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        return await ExistsAsync(g => g.Id == id, includeDeleted, cancellationToken);
    }
}
