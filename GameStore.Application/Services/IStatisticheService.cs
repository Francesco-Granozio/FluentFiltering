namespace GameStore.Application.Services;

/// <summary>
/// Interfaccia del servizio per le statistiche
/// </summary>
public interface IStatisticheService
{
    /// <summary>
    /// Ottiene le statistiche generali del sistema
    /// </summary>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Risultato con statistiche</returns>
    Task<Result<StatisticheDto>> GetStatisticheAsync(CancellationToken cancellationToken = default);
}

