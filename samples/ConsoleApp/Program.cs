using MinimalCqrs;
using Handlers.Abstractions;
using Handlers.Orders.Commands;
using Handlers.Orders.Queries;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

await RegisterServices(args);

Console.WriteLine("Hello MinimalCqrs User!");

Console.WriteLine("");
Console.WriteLine("1.Run Query");
Console.WriteLine("##########################");
Console.WriteLine("");

var listOrdersQuery = new ListOrders.Query();

var listOrdersResponse = await ConsoleApp.Execute(listOrdersQuery);

if (listOrdersResponse.IsFailure)
{
    Console.WriteLine(listOrdersResponse.Message);
}

foreach (var order in listOrdersResponse.Payload!.Orders)
{
    Console.WriteLine(order.CustomerName);
}

Console.WriteLine("");
Console.WriteLine("2.Run Command");
Console.WriteLine("##########################");
Console.WriteLine("");

var createOrderCommand = new CreateOrder.Command("Ron Swanson ", 666);

var createOrderResponse = await ConsoleApp.Execute(createOrderCommand);

if (createOrderResponse.IsFailure)
{
    Console.WriteLine(createOrderResponse.Message);
}

Console.WriteLine($"{createOrderResponse.Payload!.OrderId}, {createOrderResponse.Payload!.SomeValue}");

static async Task RegisterServices(string[] args)
{
    var logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Error)
    .WriteTo.Console()
    .CreateLogger();

    var builder = Host.CreateApplicationBuilder(args);

    builder.Services.AddSingleton<ILogger>(_ => logger);
    builder.Services.AddSerilog();
    builder.Services.AddScoped<IHandlerContext, HandlerContext>();
    builder.Services.AddMinimalCqrsFromAssemblyContaining<ListOrders.Query>();

    var host = builder.Build();
    await host.StartAsync();
}