namespace AstroCqrs;

public interface IHandlerResponse
{
}

public interface IHandlerResponse<out TResponse> : IHandlerResponse
{
    TResponse Payload { get; }
    bool IsSuccess { get; }
    bool IsFailure { get; }
    string Message { get; }
}

internal class HandlerResponse : IHandlerResponse
{
    public bool IsSuccess { get; protected set; }
    public bool IsFailure => !IsSuccess;
    public string Message { get; protected set; } = string.Empty;

    internal HandlerResponse()
    {
        IsSuccess = true;
    }

    internal HandlerResponse(string message, params object[] parameters)
    {
        IsSuccess = false;
        Message = message;
    }

    internal static IHandlerResponse CreateEmpty()
    {
        return new HandlerResponse();
    }

    internal static IHandlerResponse CreateError(string message, params object[] parameters)
    {
        return new HandlerResponse(message, parameters);
    }
}

internal sealed class HandlerResponse<TResponse> : HandlerResponse, IHandlerResponse<TResponse> where TResponse : new()
{
    internal HandlerResponse(TResponse payload)
    {
        Payload = payload;
        IsSuccess = true;
    }

    internal HandlerResponse(string message, params object[] parameters)
    {
        IsSuccess = false;
        Message = string.Format(message, parameters);
        Payload = new TResponse();
    }

    public TResponse Payload { get; private set; }

    internal static IHandlerResponse<TResponse> Create(TResponse payload)
    {
        return new HandlerResponse<TResponse>(payload);
    }

    internal new static IHandlerResponse<TResponse> CreateError(string message, params object[] parameters)
    {
        return new HandlerResponse<TResponse>(message, parameters);
    }
}