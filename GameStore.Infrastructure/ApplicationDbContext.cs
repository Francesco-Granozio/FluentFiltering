using GameStore.Domain.Entities;
using System.Linq.Expressions;

namespace GameStore.Infrastructure;

/// <summary>
/// DbContext principale dell'applicazione GameStore
/// Configurato con filtri globali per soft delete e registrazione automatica delle configurazioni EF
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // DbSet per tutte le entità
    public DbSet<Utente> Utenti => Set<Utente>();
    public DbSet<Gioco> Giochi => Set<Gioco>();
    public DbSet<Acquisto> Acquisti => Set<Acquisto>();
    public DbSet<Recensione> Recensioni => Set<Recensione>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Applica automaticamente tutte le configurazioni IEntityTypeConfiguration<T>
        // dall'assembly corrente (GameStore.Infrastructure)
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Filtro globale per soft delete
        // Questo filtro viene applicato automaticamente a tutte le query
        // per escludere le entità cancellate logicamente
        foreach (Microsoft.EntityFrameworkCore.Metadata.IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
            {
                // Crea un filtro globale che esclude le entità con IsCancellato = true
                ParameterExpression parameter = Expression.Parameter(entityType.ClrType, "e");
                MemberExpression property = Expression.Property(parameter, nameof(ISoftDelete.IsCancellato));
                LambdaExpression filter = Expression.Lambda(Expression.Equal(property, Expression.Constant(false)), parameter);

                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
            }
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        // Configurazioni aggiuntive del DbContext se necessarie
        // (es. logging, timeout, etc.)
    }
}
