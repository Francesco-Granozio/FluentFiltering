namespace GameStore.WebUI.Controllers;

/// <summary>
/// Controller per la gestione delle statistiche del sistema
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class StatisticheController : BaseController
{
    private readonly IStatisticheService _statisticheService;

    public StatisticheController(IStatisticheService statisticheService)
    {
        _statisticheService = statisticheService;
    }

    /// <summary>
    /// Ottiene le statistiche generali del sistema
    /// </summary>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Statistiche del sistema</returns>
    /// <response code="200">Statistiche recuperate con successo</response>
    /// <response code="500">Errore interno del server</response>
    [HttpGet]
    [ProducesResponseType(typeof(StatisticheDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<StatisticheDto>> GetStatisticheAsync(
        CancellationToken cancellationToken = default)
    {
        Result<StatisticheDto> result = await _statisticheService.GetStatisticheAsync(cancellationToken);
        return HandleResult(result);
    }
}
