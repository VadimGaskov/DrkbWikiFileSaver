namespace DrkbWikiFileSaver.Domain.Utils;

public class Result<T> where T : class
{
    public bool IsSuccess { get; private set; }
    public int StatusCode { get; private set; }
    public string? Error { get; private set; }
    public T Data { get; private set; }

    private Result(T value)
    {
        IsSuccess = true;
        StatusCode = 200;
        Data = value;
    }

    private Result(string error, int statusCode)
    {
        IsSuccess = false;
        Error = error;
        statusCode = statusCode;
    }

    public static Result<T> Success(T data) => new(data);
    public static Result<T> BadRequest(string errorMessage) => new(errorMessage, 404);
    public static Result<T> ServerError(string errorMessage) => new(errorMessage, 500);
}