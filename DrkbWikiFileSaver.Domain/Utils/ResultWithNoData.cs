using System.Net;

namespace DrkbWikiFileSaver.Domain.Utils;

public class Result
{
    public bool IsSuccess { get; private set; }
    public int StatusCode { get; private set; }
    public string? Error { get; private set; }

    private Result()
    {
        IsSuccess = true;
        StatusCode = 200;
    }

    private Result(string error, int statusCode)
    {
        IsSuccess = false;
        Error = error;
        StatusCode = statusCode;
    }

    public static Result Success() => new();
    public static Result BadRequest(string errorMessage) => new(errorMessage, 404);
    public static Result ServerError(string errorMessage) => new(errorMessage, 500);
}