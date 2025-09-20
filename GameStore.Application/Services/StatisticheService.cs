using GameStore.Application.Common;
using GameStore.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace GameStore.Application.Services;

/// <summary>
/// Implementazione del servizio per le statistiche
/// </summary>
public class StatisticheService : IStatisticheService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<StatisticheService> _logger;

    public StatisticheService(IUnitOfWork unitOfWork, ILogger<StatisticheService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<StatisticheDto>> GetStatisticheAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var statistiche = new StatisticheDto();

            // Conta utenti
            statistiche.TotaleUtenti = await _unitOfWork.Utenti.CountAsync(cancellationToken: cancellationToken);

            // Conta giochi
            statistiche.TotaleGiochi = await _unitOfWork.Giochi.CountAsync(cancellationToken: cancellationToken);

            // Conta acquisti
            statistiche.TotaleAcquisti = await _unitOfWork.Acquisti.CountAsync(cancellationToken: cancellationToken);

            // Conta recensioni
            statistiche.TotaleRecensioni = await _unitOfWork.Recensioni.CountAsync(cancellationToken: cancellationToken);

            // Calcola valore vendite (semplificato - potresti voler fare una query più complessa)
            var acquisti = await _unitOfWork.Acquisti.GetAllAsync(cancellationToken: cancellationToken);
            statistiche.ValoreVendite = acquisti.Sum(a => a.PrezzoPagato * a.Quantita);

            // Calcola media punteggi recensioni
            var recensioni = await _unitOfWork.Recensioni.GetAllAsync(cancellationToken: cancellationToken);
            statistiche.MediaPunteggioRecensioni = recensioni.Any() ? recensioni.Average(r => r.Punteggio) : 0;

            // Acquisti ultimo mese
            var ultimoMese = DateTime.UtcNow.AddMonths(-1);
            statistiche.AcquistiUltimoMese = acquisti.Count(a => a.DataAcquisto >= ultimoMese);

            // Recensioni ultimo mese
            statistiche.RecensioniUltimoMese = recensioni.Count(r => r.DataRecensione >= ultimoMese);

            _logger.LogInformation("Statistiche caricate: {TotaleUtenti} utenti, {TotaleGiochi} giochi, {TotaleAcquisti} acquisti", 
                statistiche.TotaleUtenti, statistiche.TotaleGiochi, statistiche.TotaleAcquisti);

            return Result<StatisticheDto>.Success(statistiche);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il caricamento delle statistiche");
            return Result<StatisticheDto>.Failure(ErrorType.UnexpectedError, "Errore durante il caricamento delle statistiche");
        }
    }
}
