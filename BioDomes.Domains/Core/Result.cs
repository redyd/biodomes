namespace BioDomes.Domains.Core;

public class Result<T>
{
    public T? Value { get; }
    public string? Error { get; }
    public ResultStatus Status { get; }

    public bool IsSuccess => Status == ResultStatus.Success;

    private Result(T value) { Value = value; Status = ResultStatus.Success; }
    private Result(ResultStatus status, string error) { Status = status; Error = error; }

    public static Result<T> Ok(T value) => new(value);
    public static Result<T> NotFound(string error) => new(ResultStatus.NotFound, error);
    public static Result<T> Forbidden(string error) => new(ResultStatus.Forbidden, error);
    public static Result<T> Conflict(string error) => new(ResultStatus.Conflict, error);
    public static Result<T> Failure(string error) => new(ResultStatus.Failure, error);
}

public enum ResultStatus { Success, NotFound, Forbidden, Conflict, Failure }