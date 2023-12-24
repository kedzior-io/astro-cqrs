namespace Handlers.Orders.Queries;
/*
 * An example of a Query with valid parameters but returning error
 */

public static class GetOrderByCustomerId
{
    public sealed record Query() : IQuery<IHandlerResponse<Response>>
    {
        public int Id { get; set; }
    }

    public record Response(OrderModel Order);

    public record OrderModel(Guid Id, decimal Total);

    public sealed class Handler : QueryHandler<Query, Response>
    {
        public Handler()
        {
        }

        public override async Task<IHandlerResponse<Response>> ExecuteAsync(Query query, CancellationToken ct)
        {
            var order = await Task.FromResult("Query Orders which return no orders");

            return Error($"Order with customer id {query.Id} not found");
        }
    }
}