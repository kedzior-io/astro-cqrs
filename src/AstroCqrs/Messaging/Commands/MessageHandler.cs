namespace AstroCqrs;

public abstract class MessageHandlerBase<TCommand> // : ValidationContext<TCommand>
{ }

public abstract class MessageHandler<TMessage> : MessageHandlerBase<TMessage>, IHandler<TMessage> where TMessage : IHandlerMessage
{
    /// <inheritdoc />
    public abstract Task ExecuteAsync(TMessage command, CancellationToken ct = default);
}

public abstract class MessageHandler<TMessage, TResult> : MessageHandlerBase<TMessage>, IMessageHandler<TMessage, TResult>
    where TMessage : IHandlerMessage<TResult>
{
    /// <inheritdoc />
    public abstract Task<TResult> ExecuteAsync(TMessage command, CancellationToken ct = default);
}