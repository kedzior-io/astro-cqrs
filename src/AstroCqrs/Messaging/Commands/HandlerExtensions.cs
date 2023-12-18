using System.Collections.Concurrent;

namespace AstroCqrs;

internal class HandlerRegistry : ConcurrentDictionary<Type, HandlerDefinition>
{ }

public static class HandlerExtensions
{
    public static Task<TResponse> ExecuteAsync<TResponse>(IHandlerMessage<TResponse> message, CancellationToken ct)
    {
        var tMessage = message.GetType();
        var registry = Conf.ServiceResolver.Resolve<HandlerRegistry>();

        if (registry.TryGetValue(tMessage, out var def))
        {
            def.HandlerExecutor ??= CreateHandlerExecutor(tMessage);

            return ((HandlerExecutorBase<TResponse>)def.HandlerExecutor).Execute(message, def.HandlerType, ct);
        }

        throw new InvalidOperationException($"Unable to create an instance of the handler for command [{tMessage.FullName}]");

        static HandlerExecutorBase<TResponse> CreateHandlerExecutor(Type tCommand)
            => (HandlerExecutorBase<TResponse>)
                Conf.ServiceResolver.CreateSingleton(Types.CommandHandlerExecutorOf2.MakeGenericType(tCommand, typeof(TResponse)));
    }

    public static Task<TResponse> ExecuteGenericAsync<TMessage, TResponse>(CancellationToken ct) where TMessage : IHandlerMessage<TResponse>
    {
        var message = Activator.CreateInstance<TMessage>();

        var tMessage = message.GetType();
        var registry = Conf.ServiceResolver.Resolve<HandlerRegistry>();

        if (registry.TryGetValue(tMessage, out var def))
        {
            def.HandlerExecutor ??= CreateHandlerExecutor(tMessage);

            return ((HandlerExecutorBase<TResponse>)def.HandlerExecutor).Execute(message, def.HandlerType, ct);
        }

        throw new InvalidOperationException($"Unable to create an instance of the handler for command [{tMessage.FullName}]");

        static HandlerExecutorBase<TResponse> CreateHandlerExecutor(Type tCommand)
            => (HandlerExecutorBase<TResponse>)
                Conf.ServiceResolver.CreateSingleton(Types.CommandHandlerExecutorOf2.MakeGenericType(tCommand, typeof(TResponse)));
    }

    //private static async Task ValidateRequest(TMessage message, List<ValidationFailure> validationFailures, CancellationToken cancellation)
    //{
    //    var valResult = await ((IValidator<TRequest>)def.GetValidator()!).ValidateAsync(req, cancellation);

    //    if (!valResult.IsValid)
    //    {
    //        for (var i = 0; i < valResult.Errors.Count; i++)
    //            validationFailures.AddError(valResult.Errors[i], def.ReqDtoFromBodyPropName);
    //    }

    //    if (validationFailures.Count > 0 && def.ThrowIfValidationFails)
    //        throw new ValidationFailureException(validationFailures, "Request validation failed");
    //}

    //internal object? GetValidator()
    //{
    //    if (_validator is null && ValidatorType is not null)
    //        _validator = Conf.ServiceResolver.CreateSingleton(ValidatorType);

    //    return _validator;
    //}
}