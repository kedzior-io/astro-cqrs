namespace AstroCqrs;

public abstract class MessageHandlerBase<TCommand>
{ }

public abstract class MessageHandler<TMessage> : MessageHandlerBase<TMessage>, IHandler<TMessage> where TMessage : IHandlerMessage
{
    public abstract Task ExecuteAsync(TMessage command, CancellationToken ct = default);
}

public abstract class MessageHandler<TMessage, TResult> : MessageHandlerBase<TMessage>, IHandler<TMessage, TResult>
    where TMessage : IHandlerMessage<TResult>
{
    public abstract Task<TResult> ExecuteAsync(TMessage command, CancellationToken ct = default);
}