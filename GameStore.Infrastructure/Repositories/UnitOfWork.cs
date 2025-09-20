namespace GameStore.Infrastructure.Repositories;

/// <summary>
/// Implementazione del Unit of Work per coordinare le operazioni sui repository
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Utenti = new UtenteRepository(_context);
        Giochi = new GiocoRepository(_context);
        Acquisti = new AcquistoRepository(_context);
        Recensioni = new RecensioneRepository(_context);
    }

    public IUtenteRepository Utenti { get; }
    public IGiocoRepository Giochi { get; }
    public IAcquistoRepository Acquisti { get; }
    public IRecensioneRepository Recensioni { get; }

    public async Task<int> SaveChangesAsync()
        => await _context.SaveChangesAsync();

    public async Task<object> BeginTransactionAsync()
        => await _context.Database.BeginTransactionAsync();

    public void Dispose()
        => _context.Dispose();
}
