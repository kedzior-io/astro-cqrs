using FluentValidation;

namespace Handlers.Orders.Commands;

/*
 * An example of a Command with parameters mapped from both querty string {orderId} and form data {someFormDataValue} and response
 */

public static class ApprovePayment
{
    public sealed record Command(string OrderId, string SomeFormDataValue) : ICommand<IHandlerResponse>;

    public sealed class OrderSubmittedValidator : Validator<Command>
    {
        public OrderSubmittedValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty();

            RuleFor(x => x.SomeFormDataValue)
                .NotEmpty();
        }
    }

    public sealed class Handler(IHandlerContext context) : CommandHandler<Command>(context)
    {
        public override async Task<IHandlerResponse> ExecuteAsync(Command command, CancellationToken ct)
        {
            await Task.FromResult("Payment Approved");

            return Success();
        }
    }
}