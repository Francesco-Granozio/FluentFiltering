using Microsoft.Extensions.DependencyInjection;

namespace GameStore.Infrastructure;

/// <summary>
/// Wrapper per gestire operazioni sicure con ApplicationDbContext
/// </summary>
public class SafeDbContextWrapper : IDisposable
{
    private readonly IServiceScope _scope;
    private readonly ApplicationDbContext _context;

    public SafeDbContextWrapper(IServiceProvider serviceProvider)
    {
        _scope = serviceProvider.CreateScope();
        _context = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    }

    public ApplicationDbContext Context => _context;

    public async Task<T> ExecuteAsync<T>(Func<ApplicationDbContext, Task<T>> operation)
    {
        try
        {
            return await operation(_context);
        }
        catch (Exception)
        {
            // Log error if needed
            throw;
        }
    }

    public async Task ExecuteAsync(Func<ApplicationDbContext, Task> operation)
    {
        try
        {
            await operation(_context);
        }
        catch (Exception)
        {
            // Log error if needed
            throw;
        }
    }

    public void Dispose()
    {
        _context?.Dispose();
        _scope?.Dispose();
    }
}
