namespace MinimalCqrs;

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

public abstract class Handler<TMessage> : IHandler<TMessage, IHandlerResponse> where TMessage : IHandlerMessage<IHandlerResponse>
{
    protected Handler()
    {
    }

    public abstract Task<IHandlerResponse> ExecuteAsync(TMessage command, CancellationToken ct = default);

    protected IHandlerResponse Success()
    {
        return HandlerResponse.CreateEmpty();
    }

    protected IHandlerResponse Error(string message)
    {
        return HandlerResponse.CreateError(message);
    }
}

public abstract class EventHandler<TMessage> : IHandler<TMessage> where TMessage : IHandlerMessage
{
    protected EventHandler()
    {
    }

    public abstract Task ExecuteAsync(TMessage command, CancellationToken ct = default);
}