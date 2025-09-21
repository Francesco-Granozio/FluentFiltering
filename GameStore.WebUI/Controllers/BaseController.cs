using FluentValidation;

namespace GameStore.WebUI.Controllers;

/// <summary>
/// Controller base con metodi helper per gestire i Result e la validazione
/// </summary>
public abstract class BaseController : ControllerBase
{
    /// <summary>
    /// Converte un Result in una risposta HTTP appropriata
    /// </summary>
    /// <typeparam name="T">Tipo del valore</typeparam>
    /// <param name="result">Risultato da convertire</param>
    /// <returns>ActionResult</returns>
    protected ActionResult<T> HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return result.Error.Type switch
        {
            ErrorType.NotFound => NotFound(new { error = result.Error.Message, type = result.Error.Type.ToString() }),
            ErrorType.ValidationFailed => BadRequest(new { error = result.Error.Message, type = result.Error.Type.ToString() }),
            ErrorType.Unauthorized => Unauthorized(new { error = result.Error.Message, type = result.Error.Type.ToString() }),
            _ => StatusCode(500, new { error = result.Error.Message, type = result.Error.Type.ToString() })
        };
    }

    /// <summary>
    /// Converte un Result senza valore in una risposta HTTP appropriata
    /// </summary>
    /// <param name="result">Risultato da convertire</param>
    /// <returns>ActionResult</returns>
    protected ActionResult HandleResult(Result result)
    {
        if (result.IsSuccess)
        {
            return Ok(new { message = "Operazione completata con successo" });
        }

        return result.Error.Type switch
        {
            ErrorType.NotFound => NotFound(new { error = result.Error.Message, type = result.Error.Type.ToString() }),
            ErrorType.ValidationFailed => BadRequest(new { error = result.Error.Message, type = result.Error.Type.ToString() }),
            ErrorType.Unauthorized => Unauthorized(new { error = result.Error.Message, type = result.Error.Type.ToString() }),
            _ => StatusCode(500, new { error = result.Error.Message, type = result.Error.Type.ToString() })
        };
    }

    /// <summary>
    /// Converte un Result con IEnumerable in una risposta HTTP appropriata
    /// </summary>
    /// <typeparam name="T">Tipo degli elementi della collezione</typeparam>
    /// <param name="result">Risultato da convertire</param>
    /// <returns>ActionResult</returns>
    protected ActionResult<IEnumerable<T>> HandleResult<T>(Result<IEnumerable<T>> result)
    {
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return result.Error.Type switch
        {
            ErrorType.NotFound => NotFound(new { error = result.Error.Message, type = result.Error.Type.ToString() }),
            ErrorType.ValidationFailed => BadRequest(new { error = result.Error.Message, type = result.Error.Type.ToString() }),
            ErrorType.Unauthorized => Unauthorized(new { error = result.Error.Message, type = result.Error.Type.ToString() }),
            _ => StatusCode(500, new { error = result.Error.Message, type = result.Error.Type.ToString() })
        };
    }

    /// <summary>
    /// Valida un DTO usando FluentValidation
    /// </summary>
    /// <typeparam name="T">Tipo del DTO</typeparam>
    /// <param name="validator">Validator per il DTO</param>
    /// <param name="dto">DTO da validare</param>
    /// <param name="cancellationToken">Token di cancellazione</param>
    /// <returns>BadRequest se la validazione fallisce, null se passa</returns>
    protected async Task<ActionResult?> ValidateAsync<T>(
        IValidator<T> validator,
        T dto,
        CancellationToken cancellationToken = default)
    {
        FluentValidation.Results.ValidationResult validationResult = await validator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            foreach (FluentValidation.Results.ValidationFailure? error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return BadRequest(ModelState);
        }
        return null;
    }
}
