using FluentValidation;

namespace Handlers.Orders.Queries;
/*
 * An example of a Query with parameters and validator
 */

public static class GetOrderByTotal
{
    public sealed record Query() : IQuery<Response>
    {
        public int TotalValue { get; set; }
    }

    public record Response(IReadOnlyCollection<OrderModel> Orders);

    public record OrderModel(Guid Id, string CustomerName, decimal Total);

    public sealed class Validator : Validator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.TotalValue)
                .GreaterThan(0);
        }
    }

    public sealed class Handler : QueryHandler<Query, Response>
    {
        public Handler()
        {
        }

        public override async Task<Response> ExecuteAsync(Query query, CancellationToken ct)
        {
            var orders = await Task.FromResult(
                new List<OrderModel>()
                {
                    new(Guid.NewGuid(),"Richard Hendricks" ,10),
                    new(Guid.NewGuid(),"Erlich Bachman" ,20),
                    new(Guid.NewGuid(),"Dinesh Chugtai" ,1000),
                }
             );

            return new Response(orders);
        }
    }
}