global using Conf = MinimalCqrs.Config;

namespace MinimalCqrs;

public sealed class Config
{
    private static IServiceResolver? _resolver;

    internal static IServiceResolver ServiceResolver
    {
        get => _resolver ?? throw new InvalidOperationException("Service Resolver not found. Did you forget to register MinimalCqrs? (builder.Services.AddMinimalCqrs())");
        set => _resolver = value;
    }
}