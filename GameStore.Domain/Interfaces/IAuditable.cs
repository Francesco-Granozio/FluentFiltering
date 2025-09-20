namespace GameStore.Domain.Interfaces;

/// <summary>
/// Interfaccia per entità che supportano auditing automatico
/// Le proprietà vengono aggiornate automaticamente tramite SaveChangesInterceptor
/// </summary>
public interface IAuditable
{
    /// <summary>
    /// Data e ora di creazione dell'entità
    /// </summary>
    DateTime DataCreazione { get; set; }

    /// <summary>
    /// Data e ora dell'ultima modifica dell'entità (null se mai modificata)
    /// </summary>
    DateTime? DataUltimaModifica { get; set; }
}
