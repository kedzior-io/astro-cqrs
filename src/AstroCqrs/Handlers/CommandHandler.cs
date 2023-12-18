namespace AstroCqrs;

public abstract class CommandHandler<TCommand, TResponse> : /*ValidationContext<TCommand>,*/ IMessageHandler<TCommand, TResponse> where TCommand : IHandlerMessage<TResponse>
{
    protected CommandHandler()
    {
    }

    public abstract Task<TResponse> ExecuteAsync(TCommand command, CancellationToken ct = default);
}