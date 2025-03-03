namespace Handlers.Users.Commands;

/*
 * An example of a Command mapped to DELETE endpoint
 */

public static class DeleteUser
{
    public sealed record Command(int UserId) : ICommand<IHandlerResponse>;

    public sealed class Handler : CommandHandler<Command>
    {
        public Handler(IHandlerContext context) : base(context)
        {
        }

        public override async Task<IHandlerResponse> ExecuteAsync(Command command, CancellationToken ct)
        {
            var deletedUser = await Task.FromResult(Guid.NewGuid());
            return Success();
        }
    }
}