using Microsoft.Extensions.DependencyInjection;

namespace GameStore.Infrastructure;

/// <summary>
/// Factory per la creazione di istanze di ApplicationDbContext
/// </summary>
public interface IDbContextFactory
{
    /// <summary>
    /// Crea una nuova istanza di ApplicationDbContext
    /// </summary>
    /// <returns>Nuova istanza di ApplicationDbContext</returns>
    ApplicationDbContext CreateDbContext();
}

/// <summary>
/// Implementazione della factory per ApplicationDbContext
/// </summary>
public class DbContextFactory : IDbContextFactory
{
    private readonly IServiceProvider _serviceProvider;

    public DbContextFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ApplicationDbContext CreateDbContext()
    {
        // Crea un nuovo scope per ogni DbContext
        IServiceScope scope = _serviceProvider.CreateScope();
        return scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    }
}
