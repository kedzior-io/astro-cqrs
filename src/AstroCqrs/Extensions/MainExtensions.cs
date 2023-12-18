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
                                            Types.IValidator
                                        }).Any()
                                )
                                );

        foreach (var discoveredType in discoveredTypes)
        {
            var interfaces = discoveredType.GetInterfaces();

            foreach (var interfaceItem in interfaces)
            {
                var generic = interfaceItem.IsGenericType ? interfaceItem.GetGenericTypeDefinition() : null;

                if (generic == Types.IMessageHandlerOf1 || generic == Types.IMessageHandlerOf2)
                {
                    handlerRegistry.TryAdd(interfaceItem.GetGenericArguments()[0], new(discoveredType));
                }

                if (interfaceItem == Types.IValidator)
                {
                    // var request = discoveredType.GetGenericArgumentsOfType(Types.ValidatorOf1)?[0]!;

                    //if (valDict.TryGetValue(tRequest, out var val))
                    //    val.HasDuplicates = true;
                    //else
                    //    valDict.Add(tRequest, new(t, false));

                    continue;
                }
            }
        }

        // TODO: check the performance of this

        var serviceProvider = services.BuildServiceProvider();

        Conf.ServiceResolver = serviceProvider.GetRequiredService<IServiceResolver>();

        //services.AddScoped<IValidator<User>, UserValidator>();

        return services;
    }
}