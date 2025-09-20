using GameStore.Domain.Entities;

namespace GameStore.Infrastructure.Repositories;

/// <summary>
/// Implementazione del repository per Utenti
/// </summary>
public class UtenteRepository : RepositoryGenerico<Utente>, IUtenteRepository
{
    public UtenteRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Utente?> GetByUsernameAsync(string username, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = QueryHelper.CreateBaseQuery(_dbSet, includeDeleted);
        return await query.FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
    }

    public async Task<Utente?> GetByEmailAsync(string email, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = QueryHelper.CreateBaseQuery(_dbSet, includeDeleted);
        return await query.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<bool> UsernameExistsAsync(string username, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = QueryHelper.CreateBaseQuery(_dbSet, false)
                              .ExcludeById(excludeId);
        return await query.AnyAsync(u => u.Username == username, cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(string email, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = QueryHelper.CreateBaseQuery(_dbSet, false)
                              .ExcludeById(excludeId);
        return await query.AnyAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid id, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        return await ExistsAsync(u => u.Id == id, includeDeleted, cancellationToken);
    }
}
