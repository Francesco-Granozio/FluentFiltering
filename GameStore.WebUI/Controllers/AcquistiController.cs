namespace GameStore.WebUI.Controllers;

/// <summary>
/// Controller per la gestione degli acquisti
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AcquistiController : BaseController
{
    private readonly IAcquistoService _acquistoService;

    public AcquistiController(IAcquistoService acquistoService)
    {
        _acquistoService = acquistoService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<AcquistoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<AcquistoDto>>> GetAcquisti([FromQuery] FilterRequest request, CancellationToken cancellationToken)
    {
        Result<PagedResult<AcquistoDto>> result = await _acquistoService.GetPagedAsync(request, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AcquistoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AcquistoDto>> GetAcquistoById(Guid id, [FromQuery] bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        Result<AcquistoDto> result = await _acquistoService.GetByIdAsync(id, includeDeleted, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("utente/{utenteId}")]
    [ProducesResponseType(typeof(IEnumerable<AcquistoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AcquistoDto>>> GetAcquistiByUtente(Guid utenteId, CancellationToken cancellationToken)
    {
        Result<IEnumerable<AcquistoDto>> result = await _acquistoService.GetByUtenteAsync(utenteId, cancellationToken);
        return HandleResult(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AcquistoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AcquistoDto>> CreateAcquisto([FromBody] CreaAcquistoDto dto, CancellationToken cancellationToken)
    {
        Result<AcquistoDto> result = await _acquistoService.CreateAsync(dto, cancellationToken);
        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(GetAcquistoById), new { id = result.Value.Id }, result.Value);
        }
        return HandleResult(result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(AcquistoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AcquistoDto>> UpdateAcquisto(Guid id, [FromBody] AggiornaAcquistoDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.Id)
        {
            return BadRequest(Error.ValidationFailed("L'ID nella rotta non corrisponde all'ID nel corpo della richiesta."));
        }
        Result<AcquistoDto> result = await _acquistoService.UpdateAsync(dto, cancellationToken);
        return HandleResult(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteAcquisto(Guid id, CancellationToken cancellationToken)
    {
        Result result = await _acquistoService.DeleteAsync(id, cancellationToken);
        if (result.IsSuccess)
        {
            return NoContent();
        }
        return HandleResult(result);
    }
}
