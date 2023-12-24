namespace Handlers.Orders.Queries;
/*
 * An example of a Query without parameters
 */

public static class ListOrders
{
    public sealed record Query() : IQuery<IHandlerResponse<Response>>;
    public record Response(IReadOnlyCollection<OrderModel> Orders);

    public record OrderModel(Guid Id, string CustomerName, decimal Total);

    public sealed class Handler : QueryHandler<Query, Response>
    {
        public Handler()
        {
        }

        public override async Task<IHandlerResponse<Response>> ExecuteAsync(Query query, CancellationToken ct)
        {
            var orders = await Task.FromResult(
                new List<OrderModel>()
                {
                    new(Guid.NewGuid(),"Richard Hendricks" ,10),
                    new(Guid.NewGuid(),"Erlich Bachman" ,20),
                    new(Guid.NewGuid(),"Dinesh Chugtai" ,1000),
                }
             );

            return Success(new Response(orders));
        }
    }
}