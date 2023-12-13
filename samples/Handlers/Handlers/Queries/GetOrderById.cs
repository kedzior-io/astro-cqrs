namespace AstroCqrs.Handlers.Queries;

/*
 * An example of a Query with a parameters
 */

public static class GetOrderById
{
    public class Query : IQuery<Response>
    {
        public string Id { get; set; } = "";
    }

    public record Response(OrderModel Order);

    public record OrderModel(Guid Id, string CustomerName, decimal Total);

    public class Handler : QueryHandler<Query, Response>
    {
        public Handler()
        {
        }

        public override async Task<Response> ExecuteAsync(Query query, CancellationToken ct)
        {
            var order = await Task.FromResult(new OrderModel(Guid.NewGuid(), "Gavin Belson", 20));

            return new Response(order);
        }
    }
}