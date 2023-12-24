namespace AstroCqrs;

internal class HandlerResponse : IHandlerResponse
{
    public bool IsSuccess { get; protected set; } = true;
    public bool IsFailure => !IsSuccess;
    public string Message { get; protected set; } = string.Empty;

    internal HandlerResponse()
    {
    }

    internal HandlerResponse(string message)
    {
        IsSuccess = false;
        Message = message;
    }

    internal static IHandlerResponse CreateEmpty()
    {
        return new HandlerResponse();
    }

    internal static IHandlerResponse CreateError(string message)
    {
        return new HandlerResponse(message);
    }
}

internal sealed class HandlerResponse<TResponse> : HandlerResponse, IHandlerResponse<TResponse>
{
    internal HandlerResponse(TResponse payload)
    {
        Payload = payload;
    }

    // TODO: Payload must containb non-nullable
    internal HandlerResponse(string message)
    {
        IsSuccess = false;
        Message = message;
    }

    public TResponse Payload { get; private set; }

    internal static IHandlerResponse<TResponse> CreateSuccess(TResponse payload)
    {
        return new HandlerResponse<TResponse>(payload);
    }

    internal new static IHandlerResponse<TResponse> CreateError(string message)
    {
        return new HandlerResponse<TResponse>(message);
    }
}