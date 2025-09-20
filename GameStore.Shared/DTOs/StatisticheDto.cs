namespace GameStore.Shared.DTOs;

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
