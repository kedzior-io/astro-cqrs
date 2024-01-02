﻿using FluentValidation;

namespace Handlers.Orders.Commands;

/*
 * An example of a Command used with Azure Service Bus
 */

public static class OrderSubmitted
{
    public sealed record Command(string Id) : ICommand<IHandlerResponse<Response>>;

    public sealed class OrderSubmittedValidator : Validator<Command>
    {
        public OrderSubmittedValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }

    public record Response(string Message);

    public sealed class Handler : CommandHandler<Command, Response>
    {
        public Handler(IHandlerContext context): base(context)
        {
        }

        public override async Task<IHandlerResponse<Response>> ExecuteAsync(Command command, CancellationToken ct)
        {
            var message = await Task.FromResult("Order confirmation email sent");
            var response = new Response(message);

            return Success(response);
        }
    }
}