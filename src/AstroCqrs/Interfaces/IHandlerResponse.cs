namespace AstroCqrs;

public interface IHandlerResponse
{
    bool IsSuccess { get; }
    bool IsFailure { get; }
    string Message { get; }
}

public interface IHandlerResponse<out TResponse> : IHandlerResponse
{
    TResponse? Payload { get; }
}