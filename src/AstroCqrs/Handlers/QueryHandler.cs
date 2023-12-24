namespace AstroCqrs;

public abstract class QueryHandlerV1<TQuery, TResponse> : IHandler<TQuery, TResponse> where TQuery : IHandlerMessage<TResponse>
{
    protected QueryHandlerV1()
    { }

    public abstract Task<TResponse> ExecuteAsync(TQuery query, CancellationToken ct = default);
}

public abstract class QueryHandler<TQuery, TResponse> : IHandler<TQuery, IHandlerResponse<TResponse>> where TQuery : IHandlerMessage<IHandlerResponse<TResponse>>
{
    protected QueryHandler()
    { }

    public abstract Task<IHandlerResponse<TResponse>> ExecuteAsync(TQuery query, CancellationToken ct = default);

    public IHandlerResponse<TResponse> Success(TResponse response)
    {
        return HandlerResponse<TResponse>.CreateSuccess(response);
    }

    public IHandlerResponse<TResponse> Error(string message)
    {
        return HandlerResponse<TResponse>.CreateError(message);
    }
}