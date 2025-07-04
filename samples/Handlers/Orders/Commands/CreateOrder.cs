﻿using FluentValidation;
using static Handlers.Orders.Events.OrderCreated;

namespace Handlers.Orders.Commands;

/*
 * An example of a Command with parameters and response
 */

public static class CreateOrder
{
    public sealed record Command(string CustomerName, decimal Total) : ICommand<IHandlerResponse<Response>>;
    public sealed record Response(Guid OrderId, string SomeValue);

    public sealed class CreateOrderValidator : Validator<Command>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.CustomerName)
                .NotNull()
                .NotEmpty();
        }
    }

    public sealed class Handler : CommandHandler<Command, Response>
    {
        public Handler(IHandlerContext context) : base(context)
        {
        }

        public override async Task<IHandlerResponse<Response>> ExecuteAsync(Command command, CancellationToken ct)
        {
            var orderId = await Task.FromResult(Guid.NewGuid());
            var response = new Response(orderId, $"{command.CustomerName}");

            var myEvent = new Event(command.CustomerName);
            await EventBus.PublishAsync(myEvent, ct);

            return Success(response);
        }
    }
}