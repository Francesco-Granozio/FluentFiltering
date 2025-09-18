using System.Linq.Expressions;
using GameStore.Domain.DTOs.Common;
using GameStore.Domain.Interfaces;
using GameStore.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using GameStore.Domain;

namespace GameStore.Infrastructure.Repositories;

/// <summary>
/// Implementazione generica del repository con filtraggio dinamico sicuro
/// </summary>
/// <typeparam name="T">Tipo dell'entità</typeparam>
public class RepositoryGenerico<T> : IRepositoryGenerico<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public RepositoryGenerico(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();
        
        if (!includeDeleted)
        {
            // Il filtro globale per soft delete è già applicato dal DbContext
            // Se includeDeleted è true, lo bypassiamo temporaneamente
            query = query.IgnoreQueryFilters();
        }

        return await query.FirstOrDefaultAsync(BuildIdExpression(id), cancellationToken);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();
        
        if (includeDeleted)
        {
            query = query.IgnoreQueryFilters();
        }

        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<Domain.DTOs.Common.PagedResult<T>> GetPagedAsync(
        FilterRequest request, 
        Expression<Func<T, bool>>? presetFilter = null, 
        Func<IQueryable<T>, IQueryable<T>>? includes = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();

        // Applica il filtro per soft delete se necessario
        if (request.IncludeDeleted)
        {
            query = query.IgnoreQueryFilters();
        }

        // Applica il filtro predefinito se fornito
        if (presetFilter != null)
        {
            query = query.Where(presetFilter);
        }

        // Applica le relazioni se fornite
        if (includes != null)
        {
            query = includes(query);
        }

        // Applica il filtro dinamico se fornito
        if (!string.IsNullOrWhiteSpace(request.Filter))
        {
            var validatedFilter = ValidateAndSanitizeFilter(request.Filter);
            query = query.Where(validatedFilter);
        }

        // Ottieni il conteggio totale prima della paginazione
        var totalItems = await query.CountAsync(cancellationToken);

        // Applica l'ordinamento se fornito
        if (!string.IsNullOrWhiteSpace(request.OrderBy))
        {
            var validatedOrderBy = ValidateAndSanitizeOrderBy(request.OrderBy);
            query = query.OrderBy(validatedOrderBy);
        }
        else
        {
            // Ordinamento predefinito per ID
            query = query.OrderBy("Id");
        }

        // Applica la paginazione
        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new GameStore.Domain.DTOs.Common.PagedResult<T>
        {
            Items = items,
            TotalItems = totalItems,
            PageSize = request.PageSize,
            PageNumber = request.PageNumber,
            TotalPages = (int)Math.Ceiling((double)totalItems / request.PageSize)
        };
    }

    public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(entities, cancellationToken);
    }

    public virtual void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public virtual void UpdateRange(IEnumerable<T> entities)
    {
        _dbSet.UpdateRange(entities);
    }

    public virtual void Remove(T entity)
    {
        // Per le entità che implementano ISoftDelete, il soft delete è gestito dall'interceptor
        // Questo metodo può essere chiamato direttamente e l'interceptor si occuperà del resto
        if (entity is ISoftDelete softDeleteEntity)
        {
            softDeleteEntity.IsCancellato = true;
            softDeleteEntity.DataCancellazione = DateTime.UtcNow;
            _dbSet.Update(entity);
        }
        else
        {
            _dbSet.Remove(entity);
        }
    }

    public virtual void RemoveRange(IEnumerable<T> entities)
    {
        foreach (var entity in entities)
        {
            Remove(entity); // Usa il metodo Remove che gestisce il soft delete
        }
    }

    public virtual void HardRemove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();
        
        if (includeDeleted)
        {
            query = query.IgnoreQueryFilters();
        }

        return await query.AnyAsync(predicate, cancellationToken);
    }

    public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();
        
        if (includeDeleted)
        {
            query = query.IgnoreQueryFilters();
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        return await query.CountAsync(cancellationToken);
    }

    #region Private Methods

    private static Expression<Func<T, bool>> BuildIdExpression(Guid id)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, "Id");
        var constant = Expression.Constant(id);
        var equality = Expression.Equal(property, constant);
        return Expression.Lambda<Func<T, bool>>(equality, parameter);
    }

    private string ValidateAndSanitizeFilter(string filter)
    {
        // Estrai i nomi dei campi dal filtro per validarli
        var fields = ExtractFieldNamesFromFilter(filter);
        
        foreach (var field in fields)
        {
            if (!FieldWhitelist.IsFieldAllowed<T>(field))
            {
                throw new ArgumentException($"Campo '{field}' non consentito per il filtraggio");
            }
        }

        return filter;
    }

    private string ValidateAndSanitizeOrderBy(string orderBy)
    {
        // Estrai i nomi dei campi dall'ordinamento per validarli
        var fields = ExtractFieldNamesFromOrderBy(orderBy);
        
        foreach (var field in fields)
        {
            if (!FieldWhitelist.IsFieldAllowed<T>(field))
            {
                throw new ArgumentException($"Campo '{field}' non consentito per l'ordinamento");
            }
        }

        return orderBy;
    }

    private static IEnumerable<string> ExtractFieldNamesFromFilter(string filter)
    {
        // Implementazione semplificata per estrarre i nomi dei campi
        // In un'implementazione reale, potresti usare un parser più sofisticato
        var fields = new HashSet<string>();
        
        // Rimuovi operatori logici e parentesi
        var cleaned = filter
            .Replace(" AND ", " ")
            .Replace(" OR ", " ")
            .Replace("(", " ")
            .Replace(")", " ")
            .Replace("=", " ")
            .Replace("!=", " ")
            .Replace(">", " ")
            .Replace("<", " ")
            .Replace(">=", " ")
            .Replace("<=", " ")
            .Replace("Contains", " ")
            .Replace("StartsWith", " ")
            .Replace("EndsWith", " ");

        // Estrai le parole che potrebbero essere nomi di campi
        var words = cleaned.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        
        foreach (var word in words)
        {
            // Rimuovi stringhe letterali e numeri
            if (!word.StartsWith("\"") && !word.StartsWith("'") && 
                !double.TryParse(word, out _) && !word.Equals("null", StringComparison.OrdinalIgnoreCase))
            {
                fields.Add(word);
            }
        }

        return fields;
    }

    private static IEnumerable<string> ExtractFieldNamesFromOrderBy(string orderBy)
    {
        var fields = new HashSet<string>();
        
        // Rimuovi "asc", "desc", virgole e spazi extra
        var cleaned = orderBy
            .Replace(" asc", "", StringComparison.OrdinalIgnoreCase)
            .Replace(" desc", "", StringComparison.OrdinalIgnoreCase)
            .Replace(",", " ");

        var words = cleaned.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        
        foreach (var word in words)
        {
            if (!string.IsNullOrWhiteSpace(word))
            {
                fields.Add(word);
            }
        }

        return fields;
    }


    #endregion
}
