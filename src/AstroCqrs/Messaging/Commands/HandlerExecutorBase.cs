namespace AstroCqrs;

internal abstract class HandlerExecutorBase
{
    internal abstract Task Execute(IHandlerMessage handlerMessage, Type handlerType, CancellationToken ct);
}

internal sealed class CommandHandlerExecutor<TCommand> : HandlerExecutorBase where TCommand : IHandlerMessage
{
    internal override Task Execute(IHandlerMessage command, Type tCommandHandler, CancellationToken ct)
        => ((IHandler<TCommand>)Conf.ServiceResolver.CreateInstance(tCommandHandler)).ExecuteAsync((TCommand)command, ct);
}

internal abstract class HandlerExecutorBase<TResult>
{
    internal abstract Task<TResult> Execute(IHandlerMessage<TResult> command, Type handlerType, CancellationToken ct);
}

internal sealed class CommandHandlerExecutor<TCommand, TResult> : HandlerExecutorBase<TResult> where TCommand : IHandlerMessage<TResult>
{
    internal override Task<TResult> Execute(IHandlerMessage<TResult> command, Type tCommandHandler, CancellationToken ct)
        => ((IHandler<TCommand, TResult>)Conf.ServiceResolver.CreateInstance(tCommandHandler)).ExecuteAsync((TCommand)command, ct);
}