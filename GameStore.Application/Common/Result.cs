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

    public static Result Success() => new(true, Error.Create("SUCCESS", "Operazione completata con successo"));
    public static Result Failure(Error error) => new(false, error);
    public static Result Failure(string code, string message) => new(false, Error.Create(code, message));
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

    public static Result<T> Success(T value) => new(true, value, Error.Create("SUCCESS", "Operazione completata con successo"));
    public new static Result<T> Failure(Error error) => new(false, default!, error);
    public new static Result<T> Failure(string code, string message) => new(false, default!, Error.Create(code, message));

    public static implicit operator Result<T>(T value) => Success(value);
    public static implicit operator Result<T>(Error error) => Failure(error);
}
