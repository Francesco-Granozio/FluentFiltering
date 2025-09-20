namespace GameStore.WebUI.Controllers;

/// <summary>
/// Controller per la gestione delle recensioni
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RecensioniController : BaseController
{
    private readonly IRecensioneService _recensioneService;

    public RecensioniController(IRecensioneService recensioneService)
    {
        _recensioneService = recensioneService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<RecensioneDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<RecensioneDto>>> GetRecensioni([FromQuery] FilterRequest request, CancellationToken cancellationToken)
    {
        Result<PagedResult<RecensioneDto>> result = await _recensioneService.GetPagedAsync(request, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RecensioneDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RecensioneDto>> GetRecensioneById(Guid id, [FromQuery] bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        Result<RecensioneDto> result = await _recensioneService.GetByIdAsync(id, includeDeleted, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("gioco/{giocoId}")]
    [ProducesResponseType(typeof(IEnumerable<RecensioneDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RecensioneDto>>> GetRecensioniByGioco(Guid giocoId, CancellationToken cancellationToken)
    {
        Result<IEnumerable<RecensioneDto>> result = await _recensioneService.GetByGiocoAsync(giocoId, cancellationToken);
        return HandleResult(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(RecensioneDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RecensioneDto>> CreateRecensione([FromBody] CreaRecensioneDto dto, CancellationToken cancellationToken)
    {
        Result<RecensioneDto> result = await _recensioneService.CreateAsync(dto, cancellationToken);
        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(GetRecensioneById), new { id = result.Value.Id }, result.Value);
        }
        return HandleResult(result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(RecensioneDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RecensioneDto>> UpdateRecensione(Guid id, [FromBody] AggiornaRecensioneDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.Id)
        {
            return BadRequest(Error.ValidationFailed("L'ID nella rotta non corrisponde all'ID nel corpo della richiesta."));
        }
        Result<RecensioneDto> result = await _recensioneService.UpdateAsync(dto, cancellationToken);
        return HandleResult(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteRecensione(Guid id, CancellationToken cancellationToken)
    {
        Result result = await _recensioneService.DeleteAsync(id, cancellationToken);
        if (result.IsSuccess)
        {
            return NoContent();
        }
        return HandleResult(result);
    }
}
