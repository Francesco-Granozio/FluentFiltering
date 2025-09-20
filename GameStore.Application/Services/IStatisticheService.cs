using GameStore.Application.Common;

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

/// <summary>
/// DTO per le statistiche del sistema
/// </summary>
public class StatisticheDto
{
    public int TotaleUtenti { get; set; }
    public int TotaleGiochi { get; set; }
    public int TotaleAcquisti { get; set; }
    public int TotaleRecensioni { get; set; }
    public decimal ValoreVendite { get; set; }
    public double MediaPunteggioRecensioni { get; set; }
    public int AcquistiUltimoMese { get; set; }
    public int RecensioniUltimoMese { get; set; }
}
