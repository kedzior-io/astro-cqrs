namespace AstroCqrs;

public abstract class Handler<TMessage, TResponse> : IHandler<TMessage, IHandlerResponse<TResponse>> where TMessage : IHandlerMessage<IHandlerResponse<TResponse>>
{
    protected Handler()
    {
    }

    public abstract Task<IHandlerResponse<TResponse>> ExecuteAsync(TMessage command, CancellationToken ct = default);

    protected IHandlerResponse<TResponse> Success(TResponse response)
    {
        return HandlerResponse<TResponse>.CreateSuccess(response);
    }

    protected IHandlerResponse<TResponse> Error(string message)
    {
        return HandlerResponse<TResponse>.CreateError(message);
    }
}