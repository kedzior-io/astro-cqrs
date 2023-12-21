# Astro CQRS

![Nuget](https://img.shields.io/nuget/v/AstroCqrs)

![Astro.CQRS](https://raw.githubusercontent.com/kedzior-io/astro-cqrs/main/astrocqrs.jpg)


Astro CQRS is a developer friendly alternative mediator implementation to [MediatR](https://github.com/jbogard/MediatR).

In-process messaging with no dependencies that allows to take decoupled, command driven approach.

It is designed to be used with:

- .NET 7
- Minimal API
- Azure Functions (HttpTrigger, ServiceBusTrigger and TimeTrigger)
- Blazor (todo)
- Console app (todo)
- MVC (todo)

## Usage

1. Install:

```csharp
dotnet install AstroCqrs
```


2. Configure :

```csharp
builder.Services.AddAstroCqrs();
```

## Query

1. Create an endpoint:

```csharp
app.MapGetHandler<GetOrderById.Query, GetOrderById.Response>("/orders.getById.{id}");
```

☝️ Simple: we are telling what's the input, the output and the path. 


2. Create a `Query`

```csharp
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
```

☝️ Simple: We keep the input, the output and executing method in a single file (not mandatory though). 

.... and that's it!

## Command

1. Create an endpoint:

```csharp
app.MapPostHandler<CreateOrder.Command, CreateOrder.Response>("/orders.create");
```

1. Create a `Command`:

```csharp
public static class CreateOrder
{
    public sealed record Command(string CustomerName, decimal Total) : ICommand<Response>;
    
    public record Response(Guid OrderId);

    public sealed class Validator : Validator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.CustomerName)
                .NotNull()
                .NotEmpty();
        }
    }

    public sealed class Handler : CommandHandler<Command, Response>
    {
        public Handler()
        {
        }

        public override async Task<Response> ExecuteAsync(Command command, CancellationToken ct)
        {
            var orderId = await Task.FromResult(Guid.NewGuid());
            return new Response(orderId, $"{command.CustomerName}");
        }
    }
}
```

☝️ Simple: Same as above + the command can be flexible and return a response


## Azure Functions

Here are the same query and command used in Azure Functions!

```csharp
services.AddAstroCqrsFromAssemblyContaining<ListOrders.Query>();
```

☝️ Ah yeah, due to the nature of Azure Functions, we need to point to the assembly where the handlers live


```csharp
public class HttpTriggerFunction
{
    [Function(nameof(HttpTriggerFunction))]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
    {
        return await AzureFunctionExtensions.ExecuteHttpPostAsync<GetOrderById.Query, GetOrderById.Response>(req);
    }
}
```


```csharp
public class ServiceBusFunction
{
    [Function(nameof(ServiceBusFunction))]
    public async Task Run([ServiceBusTrigger("created-order", Connection = "ConnectionStrings:ServiceBus")] string json, FunctionContext context)
    {
        await AzureFunctionExtensions.ExecuteServiceBusAsync<CreateOrder.Command, CreateOrder.Response>(json, JsonOptions.Defaults, context);
    }
}
```

## Sample Code

Check samples available in this repo. 

## Motives

I'm a big fan of:
- [MediatR](https://github.com/jbogard/MediatR)
- [Wolverine](https://github.com/JasperFx/wolverine)
- [FastEndpoints](https://fast-endpoints.com/)

and used them all in production environment. So why this?

Well, in all of them I was missing something:

- `MediatR` - a bit of setup + always abstracted a lot in my own wrappers.
- `Wolverine` - it covers a lot more that I need, it uses a lot of dependencies, has an odd way to setup query handler. 
- `FastEndpoints` - its command bus is amazing but the whole library enforces REPR Design Pattern (Request-Endpoint-Response) which I'm not a big fan of. It also doesn't work for Azure Functions or Blazor.

I decided to borrow the best features from existing frameworks to create an in-process messaging mechanism that features:

- Easy setup
- Decoupled and reusable handlers
- Enforced consistency
- Built-in validation
- Out-of-the-box compatibility with multiple project types (including Minimal API, Azure Functions, Console, MVC, Blazor)
- Unit testability

It can be seen in production here: [Salarioo.com](https://salarioo.com)


## TODO

There are few things to work out here and mainly:

- Customizing Handler to inject EF DbContext
- Handler Context - lazy load services needed in the most handlers aka DbContext, in memory cache etc. 
- Request Context - easily access request header values: country code, user ID etc
- Authorization example
- Blazor example
- Unit test example
- Integration test example
- Benchmarks

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## Questions, feature requests

- [Twitter](https://twitter.com/KedziorArtur)
- [Discord](https://discord.gg/j3vmcaZG)
