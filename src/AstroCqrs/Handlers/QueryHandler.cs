namespace AstroCqrs;

public abstract class QueryHandler<TQuery, TResponse> : IMessageHandler<TQuery, TResponse> where TQuery : IHandlerMessage<TResponse>
{
    protected QueryHandler()
    { }

    public abstract Task<TResponse> ExecuteAsync(TQuery query, CancellationToken ct = default);
}