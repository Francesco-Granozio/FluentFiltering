namespace GameStore.Application.Common;

/// <summary>
/// Classe base per il Result Pattern
/// </summary>
public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    protected Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, Error.Create(ErrorType.Success, "Operazione completata con successo"));
    public static Result Failure(Error error) => new(false, error);
    public static Result Failure(ErrorType errorType, string message) => new(false, Error.Create(errorType, message));

    /// <summary>
    /// Esegue una funzione basata sul risultato (successo o errore)
    /// </summary>
    /// <typeparam name="T">Tipo di ritorno</typeparam>
    /// <param name="onSuccess">Funzione da eseguire in caso di successo</param>
    /// <param name="onError">Funzione da eseguire in caso di errore</param>
    /// <returns>Risultato della funzione eseguita</returns>
    public T Match<T>(Func<T> onSuccess, Func<Error, T> onError)
    {
        return IsSuccess ? onSuccess() : onError(Error);
    }

    /// <summary>
    /// Esegue un'azione basata sul risultato (successo o errore)
    /// </summary>
    /// <param name="onSuccess">Azione da eseguire in caso di successo</param>
    /// <param name="onError">Azione da eseguire in caso di errore</param>
    public void Match(Action onSuccess, Action<Error> onError)
    {
        if (IsSuccess)
            onSuccess();
        else
            onError(Error);
    }
}

/// <summary>
/// Classe generica per il Result Pattern con valore di ritorno
/// </summary>
/// <typeparam name="T">Tipo del valore di ritorno</typeparam>
public class Result<T> : Result
{
    public T Value { get; }

    private Result(bool isSuccess, T value, Error error) : base(isSuccess, error)
    {
        Value = value;
    }

    public static Result<T> Success(T value) => new(true, value, Error.Create(ErrorType.Success, "Operazione completata con successo"));
    public new static Result<T> Failure(Error error) => new(false, default!, error);
    public new static Result<T> Failure(ErrorType errorType, string message) => new(false, default!, Error.Create(errorType, message));

    public static implicit operator Result<T>(T value) => Success(value);
    public static implicit operator Result<T>(Error error) => Failure(error);

    /// <summary>
    /// Esegue una funzione basata sul risultato (successo o errore) con accesso al valore
    /// </summary>
    /// <typeparam name="TResult">Tipo di ritorno</typeparam>
    /// <param name="onSuccess">Funzione da eseguire in caso di successo con accesso al valore</param>
    /// <param name="onError">Funzione da eseguire in caso di errore</param>
    /// <returns>Risultato della funzione eseguita</returns>
    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Error, TResult> onError)
    {
        return IsSuccess ? onSuccess(Value) : onError(Error);
    }

    /// <summary>
    /// Esegue un'azione basata sul risultato (successo o errore) con accesso al valore
    /// </summary>
    /// <param name="onSuccess">Azione da eseguire in caso di successo con accesso al valore</param>
    /// <param name="onError">Azione da eseguire in caso di errore</param>
    public void Match(Action<T> onSuccess, Action<Error> onError)
    {
        if (IsSuccess)
            onSuccess(Value);
        else
            onError(Error);
    }
}
