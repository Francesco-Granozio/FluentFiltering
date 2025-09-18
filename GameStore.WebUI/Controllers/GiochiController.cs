using GameStore.Application.DTOs;
using GameStore.Domain.DTOs.Common;
using GameStore.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.WebUI.Controllers;

/// <summary>
/// Controller per la gestione dei giochi
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class GiochiController : BaseController
{
    private readonly IGiocoService _giocoService;

    public GiochiController(IGiocoService giocoService)
    {
        _giocoService = giocoService;
    }

    /// <summary>
    /// Ottiene giochi paginati con filtro
    /// </summary>
    /// <param name="request">Richiesta di filtro e paginazione</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Lista paginata di giochi</returns>
    [HttpGet]
    public async Task<ActionResult<PagedResult<GiocoDto>>> GetPagedAsync(
        [FromQuery] FilterRequest request, 
        CancellationToken cancellationToken = default)
    {
        var result = await _giocoService.GetPagedAsync(request, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Ottiene un gioco per ID
    /// </summary>
    /// <param name="id">ID del gioco</param>
    /// <param name="includeDeleted">Indica se includere giochi cancellati</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Gioco trovato</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<GiocoDto>> GetByIdAsync(
        Guid id, 
        [FromQuery] bool includeDeleted = false, 
        CancellationToken cancellationToken = default)
    {
        var result = await _giocoService.GetByIdAsync(id, includeDeleted, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Crea un nuovo gioco
    /// </summary>
    /// <param name="dto">DTO per la creazione</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Gioco creato</returns>
    [HttpPost]
    public async Task<ActionResult<GiocoDto>> CreateAsync(
        [FromBody] CreaGiocoDto dto, 
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _giocoService.CreateAsync(dto, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Aggiorna un gioco esistente
    /// </summary>
    /// <param name="id">ID del gioco</param>
    /// <param name="dto">DTO per l'aggiornamento</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Gioco aggiornato</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<GiocoDto>> UpdateAsync(
        Guid id, 
        [FromBody] AggiornaGiocoDto dto, 
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != dto.Id)
        {
            return BadRequest("L'ID nell'URL non corrisponde all'ID nel corpo della richiesta");
        }

        var result = await _giocoService.UpdateAsync(dto, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Cancella un gioco (soft delete)
    /// </summary>
    /// <param name="id">ID del gioco</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Risultato dell'operazione</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(
        Guid id, 
        CancellationToken cancellationToken = default)
    {
        var result = await _giocoService.DeleteAsync(id, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Ottiene i giochi più venduti
    /// </summary>
    /// <param name="count">Numero di giochi da restituire</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Lista dei giochi più venduti</returns>
    [HttpGet("top-selling")]
    public async Task<ActionResult<IEnumerable<GiocoDto>>> GetTopSellingAsync(
        [FromQuery] int count = 10, 
        CancellationToken cancellationToken = default)
    {
        var result = await _giocoService.GetTopSellingAsync(count, cancellationToken);
        return HandleResult(result);
    }
}
