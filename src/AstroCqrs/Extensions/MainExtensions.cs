using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
        var validators = new Dictionary<Type, Type>();

        services.AddHttpContextAccessor();
        services.AddSingleton(handlerRegistry);
        services.AddSingleton(validators);
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
                    validators.Add(messageType, discoveredType);
                    // services.AddScoped<IValidator<messageType>, discoveredType>();
                }
            }
        }

        // TODO: check the performance of this

        var serviceProvider = services.BuildServiceProvider();

        Conf.ServiceResolver = serviceProvider.GetRequiredService<IServiceResolver>();
        
        return services;
    }
}