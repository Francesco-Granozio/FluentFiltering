using GameStore.Application.DTOs;
using GameStore.Domain.DTOs.Common;
using GameStore.Application.Services;
using GameStore.WebUI.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.WebUI.Controllers;

/// <summary>
/// Controller per la gestione degli utenti
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UtentiController : BaseController
{
    private readonly IUtenteService _utenteService;

    public UtentiController(IUtenteService utenteService)
    {
        _utenteService = utenteService;
    }

    /// <summary>
    /// Ottiene utenti paginati con filtro
    /// </summary>
    /// <param name="request">Richiesta di filtro e paginazione</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Lista paginata di utenti</returns>
    [HttpGet]
    public async Task<ActionResult<PagedResult<UtenteDto>>> GetPagedAsync(
        [FromQuery] FilterRequest request, 
        CancellationToken cancellationToken = default)
    {
        var result = await _utenteService.GetPagedAsync(request, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Ottiene un utente per ID
    /// </summary>
    /// <param name="id">ID dell'utente</param>
    /// <param name="includeDeleted">Indica se includere utenti cancellati</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Utente trovato</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<UtenteDto>> GetByIdAsync(
        Guid id, 
        [FromQuery] bool includeDeleted = false, 
        CancellationToken cancellationToken = default)
    {
        var result = await _utenteService.GetByIdAsync(id, includeDeleted, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Crea un nuovo utente
    /// </summary>
    /// <param name="dto">DTO per la creazione</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Utente creato</returns>
    [HttpPost]
    public async Task<ActionResult<UtenteDto>> CreateAsync(
        [FromBody] CreaUtenteDto dto, 
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _utenteService.CreateAsync(dto, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Aggiorna un utente esistente
    /// </summary>
    /// <param name="id">ID dell'utente</param>
    /// <param name="dto">DTO per l'aggiornamento</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Utente aggiornato</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<UtenteDto>> UpdateAsync(
        Guid id, 
        [FromBody] AggiornaUtenteDto dto, 
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

        var result = await _utenteService.UpdateAsync(dto, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Cancella un utente (soft delete)
    /// </summary>
    /// <param name="id">ID dell'utente</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>Risultato dell'operazione</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(
        Guid id, 
        CancellationToken cancellationToken = default)
    {
        var result = await _utenteService.DeleteAsync(id, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Verifica se un utente esiste
    /// </summary>
    /// <param name="id">ID dell'utente</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>True se l'utente esiste</returns>
    [HttpGet("{id}/exists")]
    public async Task<ActionResult<bool>> ExistsAsync(
        Guid id, 
        CancellationToken cancellationToken = default)
    {
        var result = await _utenteService.ExistsAsync(id, cancellationToken);
        return HandleResult(result);
    }
}
