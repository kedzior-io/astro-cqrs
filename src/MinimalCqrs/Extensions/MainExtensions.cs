using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace MinimalCqrs;

public static class MainExtensions
{
    public static IServiceCollection AddMinimalCqrsFromAssemblyContaining<T>(this IServiceCollection services)
    {
        return RegisterMinimalCqrs(services, typeof(T).Assembly);
    }

    public static IServiceCollection AddMinimalCqrs(this IServiceCollection services)
    {
        return RegisterMinimalCqrs(services);
    }

    private static IServiceCollection RegisterMinimalCqrs(this IServiceCollection services, Assembly? assembly = null)
    {
        var handlerRegistry = new HandlerRegistry();
        var validatorRegistry = new ValidatorRegistry();

        services.AddHttpContextAccessor();
        services.AddSingleton(handlerRegistry);
        services.AddSingleton(validatorRegistry);
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
                                            Types.IEvent,
                                            Types.IMessageHandler,
                                            Types.IValidator
                                        }).Any()
                                )
                                );

        foreach (var discoveredType in discoveredTypes)
        {
            var interfaceTypes = discoveredType.GetInterfaces();

            foreach (var interfaceType in interfaceTypes)
            {
                var genericType = interfaceType.IsGenericType ? interfaceType.GetGenericTypeDefinition() : null;

                if (genericType == Types.IMessageHandlerOf1 || genericType == Types.IMessageHandlerOf2)
                {
                    handlerRegistry.TryAdd(interfaceType.GetGenericArguments()[0], new(discoveredType));
                }

                if (interfaceType == Types.IValidator)
                {
                    var messageType = discoveredType.GetGenericArgumentsOfType(Types.ValidatorOf1)?[0]!;
                    validatorRegistry.TryAdd(messageType, discoveredType);
                }
            }
        }

        var serviceProvider = services.BuildServiceProvider();

        Conf.ServiceResolver = serviceProvider.GetRequiredService<IServiceResolver>();

        return services;
    }
}