using GameStore.Application.Services;
using GameStore.Shared.Common;
using GameStore.Shared.DTOs;
using GameStore.Shared.DTOs.Common;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.WebUI.Controllers;

/// <summary>
/// Controller per la gestione dei giochi acquistati
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class GiochiAcquistatiController : BaseController
{
    private readonly IGiochiAcquistatiService _giochiAcquistatiService;

    public GiochiAcquistatiController(IGiochiAcquistatiService giochiAcquistatiService)
    {
        _giochiAcquistatiService = giochiAcquistatiService;
    }

    /// <summary>
    /// Ottiene i giochi acquistati paginati
    /// </summary>
    /// <param name="request">Richiesta di filtro e paginazione</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Risultato paginato dei giochi acquistati</returns>
    [HttpGet("paged")]
    public async Task<ActionResult<PagedResult<GiochiAcquistatiDto>>> GetPagedAsync(
        [FromQuery] FilterRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _giochiAcquistatiService.GetPagedAsync(request, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Ottiene tutti i giochi acquistati
    /// </summary>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Lista dei giochi acquistati</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GiochiAcquistatiDto>>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var result = await _giochiAcquistatiService.GetAllAsync(cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Ottiene i giochi acquistati con filtro personalizzato
    /// </summary>
    /// <param name="filter">Filtro di ricerca (opzionale)</param>
    /// <param name="orderBy">Ordinamento (opzionale)</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Lista dei giochi acquistati filtrati</returns>
    [HttpGet("filtered")]
    public async Task<ActionResult<IEnumerable<GiochiAcquistatiDto>>> GetFilteredAsync(
        [FromQuery] string? filter = null,
        [FromQuery] string? orderBy = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _giochiAcquistatiService.GetFilteredAsync(filter, orderBy, cancellationToken);
        return HandleResult(result);
    }
}
