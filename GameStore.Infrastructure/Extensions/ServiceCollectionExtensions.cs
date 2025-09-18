using GameStore.Domain.Interfaces;
using GameStore.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GameStore.Infrastructure.Extensions;

/// <summary>
/// Extension methods per la registrazione dei servizi Infrastructure
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registra tutti i servizi Infrastructure
    /// </summary>
    /// <param name="services">Collezione di servizi</param>
    /// <param name="configuration">Configurazione dell'applicazione</param>
    /// <returns>Collezione di servizi</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Registrazione DbContext (già presente in Program.cs, ma lo includiamo per completezza)
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Registrazione Repository generico
        services.AddScoped(typeof(IRepositoryGenerico<>), typeof(RepositoryGenerico<>));

        // Registrazione Repository specifici
        services.AddScoped<IUtenteRepository, UtenteRepository>();
        services.AddScoped<IGiocoRepository, GiocoRepository>();
        services.AddScoped<IAcquistoRepository, AcquistoRepository>();
        services.AddScoped<IRecensioneRepository, RecensioneRepository>();

        // Registrazione Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
