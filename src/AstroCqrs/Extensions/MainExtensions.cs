using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Linq;
using System.Reflection;

namespace AstroCqrs;

public static class MainExtensions
{
    public static IServiceCollection AddAstroCqrsFromAssemblyContaining<T>(this IServiceCollection services)
    {
        return RegisterAstroCqrs(services, typeof(T).Assembly);
    }

    public static IServiceCollection AddAstroCqrs(this IServiceCollection services)
    {
        return RegisterAstroCqrs(services);
    }

    private static IServiceCollection RegisterAstroCqrs(this IServiceCollection services, Assembly? assembly = null)
    {
        var handlerRegistry = new HandlerRegistry();

        services.AddHttpContextAccessor();
        services.AddSingleton(handlerRegistry);
        services.TryAddSingleton<IServiceResolver, ServiceResolver>();

        var allAssemblies = Enumerable.Empty<Assembly>();

        allAssemblies = allAssemblies.Union(AppDomain.CurrentDomain.GetAssemblies());

        if (assembly is not null)
        {
            allAssemblies = allAssemblies.Append(assembly);
        }

        var discoveredTypes = allAssemblies
                                .SelectMany(a => a.GetTypes()
                                .Where(
                                    t =>
                                    t is { IsAbstract: false, IsInterface: false, IsGenericType: false } &&
                                    t.GetInterfaces().Intersect(
                                        new[]
                                        {
                                            Types.IQuery,
                                            Types.ICommand,
                                            Types.IMessageHandler,
                                        }).Any()
                                )
                                );

        foreach (var t in discoveredTypes)
        {
            var tInterfaces = t.GetInterfaces();

            foreach (var tInterface in tInterfaces)
            {
                var tGeneric = tInterface.IsGenericType ? tInterface.GetGenericTypeDefinition() : null;

                if (tGeneric == Types.IMessageHandlerOf1 || tGeneric == Types.IMessageHandlerOf2)
                {
                    handlerRegistry.TryAdd(tInterface.GetGenericArguments()[0], new(t));
                }
            }
        }

        // TODO: check the performance of this

        var serviceProvider = services.BuildServiceProvider();

        Conf.ServiceResolver = serviceProvider.GetRequiredService<IServiceResolver>();

        return services;
    }
}