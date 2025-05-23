﻿namespace MinimalCqrs;

public interface IHandler
{ }

public interface IHandler<in TMessage> : IHandler where TMessage : IHandlerMessage
{
    Task ExecuteAsync(TMessage message, CancellationToken ct);
}

public interface IHandler<in TMessage, TResult> : IHandler where TMessage : IHandlerMessage<TResult>
{
    Task<TResult> ExecuteAsync(TMessage message, CancellationToken ct);
}