# Astro CQRS

An alternative mediator implementation to [MediatR](https://github.com/jbogard/MediatR).

In-process messaging with no dependencies that allows to take decoupled, command driven approach.

It is designed to be used with:

- .net 7
- Minimal API
- Azure Functions
- Blazor


## Usage

1. Install:

```csharp
// dotnet install Astro.Cqrs
```

☝️ Ah yeah that's not on NuGet yet.


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


2. Create a `query`

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

1. Create a command:

```csharp
public static class CreateOrder
{
    public sealed record Command(string CustomerName, decimal Total) : ICommand<Response>;
    public record Response(Guid OrderId);

    public sealed class Handler : CommandHandler<Command, Response>
    {
        public Handler()
        {
        }

        public override async Task<Response> ExecuteAsync(Command command, CancellationToken ct)
        {
            var orderId = await Task.FromResult(Guid.NewGuid());
            return new Response(orderId);
        }
    }
}
```

☝️ Simple: Same as above + the command can be flexible and return a response (it's mandatory, it can be fire & forget)


## Azure Functions

Here are the same query and command used in Azure Functions!

```csharp
     services.AddAstroCqrsFromAssemblyContaining<ListOrders.Query>();
```

☝️ Ah yeah, due to the nature of Azure Functions, we need to point to the assembly where the handlers live


```csharp
public class HttpTriggerFunction
{
    private readonly ILogger<HttpTriggerFunction> _logger;

    public HttpTriggerFunction(ILogger<HttpTriggerFunction> logger)
    {
        _logger = logger;
    }

    [Function(nameof(HttpTriggerFunction))]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
    {
        return await AzureFunctionExtensions.ExecuteAsync<GetOrderById.Query, GetOrderById.Response>(req);
    }
}
```


```csharp
public class ServiceBusFunction
{
    private readonly ILogger<ServiceBusFunction> _logger;

    public ServiceBusFunction(ILogger<ServiceBusFunction> logger)
    {
        _logger = logger;
    }

    [Function(nameof(ServiceBusFunction))]
    public async Task Run([ServiceBusTrigger("created-order", Connection = "ConnectionStrings:ServiceBus")] string json, FunctionContext context)
    {
        await AzureFunctionExtensions.ExecuteAsync<CreateOrder.Command, CreateOrder.Response>(json, JsonOptions.Defaults, context);
    }
}
```

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

I decided to borrow the best parts from them in order to create a setup free in-process messaging mechanism that wires up easily with Minimal API, Azure Functions, Blazor and MVC (if I get to do it soon enough). 

It can be seen in production here: [Salarioo.com](https://salarioo.com)


## TODO

There are few things to work out here and mainly:

- add Command automatic valiadtion using FluentValidation
- add customizing Handler to inject DbContext
- add Handler Context
- add Request Context
- add authorization example
- add Blazor example
- add unit test example
- add integration test example

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.