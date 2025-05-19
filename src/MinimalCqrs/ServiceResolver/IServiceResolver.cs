using Microsoft.Extensions.DependencyInjection;

namespace MinimalCqrs;

public interface IServiceResolverBase
{
    IServiceScope CreateScope();

    TService? TryResolve<TService>() where TService : class;

    object? TryResolve(Type typeOfService);

    TService Resolve<TService>() where TService : class;

    object Resolve(Type typeOfService);
}

public interface IServiceResolver : IServiceResolverBase
{
    object CreateInstance(Type type, IServiceProvider? serviceProvider = null);

    object CreateSingleton(Type type);
}