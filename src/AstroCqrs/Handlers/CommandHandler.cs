namespace AstroCqrs;

public abstract class CommandHandlerV1<TCommand, TResponse> : IHandler<TCommand, TResponse> where TCommand : IHandlerMessage<TResponse>
{
    protected CommandHandlerV1()
    {
    }

    public abstract Task<TResponse> ExecuteAsync(TCommand command, CancellationToken ct = default);
}

public abstract class CommandHandler<TCommand, TResponse> : IHandler<TCommand, IHandlerResponse<TResponse>> where TCommand : IHandlerMessage<IHandlerResponse<TResponse>>
{
    protected CommandHandler()
    {
    }

    public abstract Task<IHandlerResponse<TResponse>> ExecuteAsync(TCommand command, CancellationToken ct = default);

    public IHandlerResponse<TResponse> Success(TResponse response)
    {
        return HandlerResponse<TResponse>.CreateSuccess(response);
    }

    public IHandlerResponse<TResponse> Error(string message)
    {
        return HandlerResponse<TResponse>.CreateError(message);
    }
}