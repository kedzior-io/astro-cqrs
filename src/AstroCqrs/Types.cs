using FluentValidation;

namespace AstroCqrs;

internal static class Types
{
    internal static readonly Type IQuery = typeof(IQuery);
    internal static readonly Type ICommand = typeof(ICommand);

    internal static readonly Type IMessageHandler = typeof(IMessageHandler);
    internal static readonly Type IMessageHandlerOf1 = typeof(IMessageHandler<>);

    internal static readonly Type IMessageHandlerOf2 = typeof(IMessageHandler<,>);
    internal static readonly Type CommandHandlerExecutorOf1 = typeof(CommandHandlerExecutor<>);
    internal static readonly Type CommandHandlerExecutorOf2 = typeof(CommandHandlerExecutor<,>);

    internal static readonly Type IValidator = typeof(IValidator);
}