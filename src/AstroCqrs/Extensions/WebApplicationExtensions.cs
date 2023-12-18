using Azure;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AstroCqrs;

public static class WebApplicationExtensions
{
    public static RouteHandlerBuilder MapGetHandler<TQuery, TResponse>(this WebApplication app, string pattern) where TQuery : IHandlerMessage<TResponse>
    {
        return app.MapGet(pattern, async ([AsParameters] TQuery query, CancellationToken ct) => await HandlerExtensions.ExecuteAsync(query, ct));
    }

    public static RouteHandlerBuilder MapPostHandler<TCommand, TResponse>(this WebApplication app, string pattern) where TCommand : IHandlerMessage<TResponse>
    {
        return app.MapPost(pattern, async (TCommand? command, CancellationToken ct) =>
        {
            if (command is not null)
            {
                await ExecuteValidation(command);

                return await HandlerExtensions.ExecuteAsync(command, ct);
            }

            return await HandlerExtensions.ExecuteGenericAsync<TCommand, TResponse>(ct);
        });
    }

    private static async Task ExecuteValidation<TResponse>(IHandlerMessage<TResponse> message)
    {
        var messageType = message.GetType();

        var validators = Conf.ServiceResolver.Resolve<ValidatorRegistry>();

        if (validators.TryGetValue(messageType, out var validatorType))
        {
            // var validatorResults = ((IValidator)GetValidator(validatorType)!).ValidateAsync(message);
        }
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

    internal static object? _validator = null;

    internal static object? GetValidator(Type validatorType)
    {
        _validator ??= Conf.ServiceResolver.CreateSingleton(validatorType);

        return _validator;
    }
}