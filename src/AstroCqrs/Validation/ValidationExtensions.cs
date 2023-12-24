using FluentValidation;

namespace AstroCqrs;

internal static class ValidationExtensions
{
    internal static IValidator? _validator = null;

    public static async Task<IDictionary<string, string[]>?> ExecuteValidationAsync<TResponse>(IHandlerMessage<TResponse> message)
    {
        var messageType = message.GetType();

        var validators = Conf.ServiceResolver.Resolve<ValidatorRegistry>();

        if (validators.TryGetValue(messageType, out var validatorType))
        {
            var validationContext = new ValidationContext<IHandlerMessage<TResponse>>(message);
            var validationResult = await GetValidator(validatorType).ValidateAsync(validationContext);

            return validationResult.IsValid ? null : validationResult.ToDictionary();
        }

        return null;
    }

    public static async Task<IDictionary<string, string[]>?> ExecuteValidation<TMessage, TResponse>() where TMessage : IHandlerMessage<TResponse>
    {
        var message = Activator.CreateInstance<TMessage>();
        return await ExecuteValidationAsync(message);
    }

    internal static IValidator GetValidator(Type validatorType)
    {
        _validator = (IValidator)Conf.ServiceResolver.CreateSingleton(validatorType);
        return _validator;
    }
}