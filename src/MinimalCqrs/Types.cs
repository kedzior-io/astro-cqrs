using FluentValidation;

namespace MinimalCqrs;

internal static class Types
{
    internal static readonly Type IQuery = typeof(IQuery);
    internal static readonly Type ICommand = typeof(ICommand);

    internal static readonly Type IMessageHandler = typeof(IHandler);
    internal static readonly Type IMessageHandlerOf1 = typeof(IHandler<>);

    internal static readonly Type IMessageHandlerOf2 = typeof(IHandler<,>);
    internal static readonly Type CommandHandlerExecutorOf1 = typeof(CommandHandlerExecutor<>);
    internal static readonly Type CommandHandlerExecutorOf2 = typeof(CommandHandlerExecutor<,>);

    internal static readonly Type IValidator = typeof(IValidator);
    internal static readonly Type ValidatorOf1 = typeof(AbstractValidator<>);
}