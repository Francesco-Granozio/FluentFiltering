namespace GameStore.Domain;

/// <summary>
/// Interfaccia per entità che supportano soft delete
/// Le proprietà vengono gestite automaticamente tramite SaveChangesInterceptor
/// </summary>
public interface ISoftDelete
{
    /// <summary>
    /// Indica se l'entità è stata cancellata logicamente
    /// </summary>
    bool IsCancellato { get; set; }

    /// <summary>
    /// Data e ora della cancellazione logica (null se non cancellata)
    /// </summary>
    DateTime? DataCancellazione { get; set; }
}
