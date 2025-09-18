namespace GameStore.Domain.Interfaces;

/// <summary>
/// Interfaccia Unit of Work per coordinare le operazioni sui repository
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IUtenteRepository Utenti { get; }
    IGiocoRepository Giochi { get; }
    IAcquistoRepository Acquisti { get; }
    IRecensioneRepository Recensioni { get; }

    Task<int> SaveChangesAsync();
    Task<object> BeginTransactionAsync();
}
