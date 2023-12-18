global using Conf = AstroCqrs.Config;
using FluentValidation;

// ReSharper disable MemberCanBeMadeStatic.Global
#pragma warning disable RCS1102,CA1822

namespace AstroCqrs;

public sealed class Config
{
    private static IServiceResolver? _resolver;
    // internal static bool ResolverIsNotSet => _resolver is null;

    internal static IServiceResolver ServiceResolver
    {
        get => _resolver ?? throw new InvalidOperationException("Service resolver is null");
        set => _resolver = value;
    }

    internal static readonly ValidationOptions ValOpts = new();
}