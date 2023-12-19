global using Conf = AstroCqrs.Config;

namespace AstroCqrs;

public sealed class Config
{
    private static IServiceResolver? _resolver;

    internal static IServiceResolver ServiceResolver
    {
        get => _resolver ?? throw new InvalidOperationException("Service resolver is null");
        set => _resolver = value;
    }

    internal static readonly ValidationOptions ValOpts = new();
}