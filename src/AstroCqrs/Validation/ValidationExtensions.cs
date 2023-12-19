using FluentValidation;

namespace AstroCqrs;

internal static class ValidationExtensions
{
    internal static IValidator? _validator = null;

    public static async Task ExecuteValidationAsync<TResponse>(IHandlerMessage<TResponse> message)
    {
        var messageType = message.GetType();

        var validators = Conf.ServiceResolver.Resolve<ValidatorRegistry>();

        if (validators.TryGetValue(messageType, out var validatorType))
        {
            var validationContext = new ValidationContext<IHandlerMessage<TResponse>>(message);
            var validatorResults = await GetValidator(validatorType).ValidateAsync(validationContext);

            if (!validatorResults.IsValid)
            {
                for (var i = 0; i < validatorResults.Errors.Count; i++)
                {
                }
            }
        }
    }

    public static async Task ExecuteValidation<TMessage, TResponse>() where TMessage : IHandlerMessage<TResponse>
    {
        var message = Activator.CreateInstance<TMessage>();
        await ExecuteValidationAsync(message);
    }

    internal static IValidator GetValidator(Type validatorType)
    {
        _validator ??= (IValidator)Conf.ServiceResolver.CreateSingleton(validatorType);
        return _validator;
    }
}