using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Handlers.Orders.Commands;

/*
 * An example of a Command with parameters mapped from both querty string {orderId} and body {reference}
 */

public static class ConfirmPayment
{
    public sealed record Command([AsParameters] string Id, string Reference) : ICommand<IHandlerResponse>;

    public sealed class OrderSubmittedValidator : Validator<Command>
    {
        public OrderSubmittedValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Reference)
                .NotEmpty();
        }
    }

    public sealed class Handler(IHandlerContext context) : CommandHandler<Command>(context)
    {
        public override async Task<IHandlerResponse> ExecuteAsync(Command command, CancellationToken ct)
        {
            await Task.FromResult("Order payment confirmed");

            return Success();
        }
    }
}