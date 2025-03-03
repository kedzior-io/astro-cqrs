using FluentValidation;

namespace Handlers.Users.Commands;

/*
 * An example of a Command mapped to PUT endpoint
 */

public static class UpdateUser
{
    public sealed record Command(string Username) : ICommand<IHandlerResponse<Response>>;
    public sealed record Response(Guid UserId);

    public sealed class CreateOrderValidator : Validator<Command>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.Username)
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
            var customerId = await Task.FromResult(Guid.NewGuid());
            var response = new Response(customerId);

            return Success(response);
        }
    }
}