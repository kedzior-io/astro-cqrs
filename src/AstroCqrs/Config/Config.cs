global using Conf = AstroCqrs.Config;

namespace AstroCqrs;

public sealed class Config
{
    private static IServiceResolver? _resolver;

    internal static IServiceResolver ServiceResolver
    {
        get => _resolver ?? throw new InvalidOperationException("Service Resolver not found. Did you forget to register AstroCQRS? (builder.Services.AddAstroCqrs())");
        set => _resolver = value;
    }
}