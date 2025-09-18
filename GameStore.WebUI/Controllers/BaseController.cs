using GameStore.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.WebUI.Controllers;

/// <summary>
/// Controller base con metodi helper per gestire i Result
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

        return result.Error.Code switch
        {
            "NOT_FOUND" => NotFound(new { error = result.Error.Message, code = result.Error.Code }),
            "VALIDATION_FAILED" => BadRequest(new { error = result.Error.Message, code = result.Error.Code }),
            "UNAUTHORIZED" => Unauthorized(new { error = result.Error.Message, code = result.Error.Code }),
            _ => StatusCode(500, new { error = result.Error.Message, code = result.Error.Code })
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

        return result.Error.Code switch
        {
            "NOT_FOUND" => NotFound(new { error = result.Error.Message, code = result.Error.Code }),
            "VALIDATION_FAILED" => BadRequest(new { error = result.Error.Message, code = result.Error.Code }),
            "UNAUTHORIZED" => Unauthorized(new { error = result.Error.Message, code = result.Error.Code }),
            _ => StatusCode(500, new { error = result.Error.Message, code = result.Error.Code })
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

        return result.Error.Code switch
        {
            "NOT_FOUND" => NotFound(new { error = result.Error.Message, code = result.Error.Code }),
            "VALIDATION_FAILED" => BadRequest(new { error = result.Error.Message, code = result.Error.Code }),
            "UNAUTHORIZED" => Unauthorized(new { error = result.Error.Message, code = result.Error.Code }),
            _ => StatusCode(500, new { error = result.Error.Message, code = result.Error.Code })
        };
    }
}
