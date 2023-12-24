namespace Handlers.Orders.Queries;

/*
 * An example of a Query with a parameters
 */

public static class GetOrderById
{
    public class Query : IQuery<IHandlerResponse<Response>>
    {
        public string Id { get; set; } = "";
    }

    public record Response(OrderModel Order);

    public record OrderModel(string Id, string CustomerName, decimal Total);

    public class Handler : QueryHandler<Query, Response>
    {
        public Handler()
        {
        }

        public override async Task<IHandlerResponse<Response>> ExecuteAsync(Query query, CancellationToken ct)
        {
            var order = await Task.FromResult(new OrderModel(query.Id, "Gavin Belson", 20));

            return Success(new Response(order));
        }
    }
}