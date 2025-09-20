namespace GameStore.Domain.Interfaces;

/// <summary>
/// Interfaccia base per tutte le entità del dominio con identificatore tipizzato
/// </summary>
/// <typeparam name="TId">Tipo dell'identificatore (es. Guid, int)</typeparam>
public interface IEntity<TId> where TId : struct
{
    /// <summary>
    /// Identificatore univoco dell'entità
    /// </summary>
    TId Id { get; set; }
}
