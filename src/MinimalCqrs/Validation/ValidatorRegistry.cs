using System.Collections.Concurrent;

namespace MinimalCqrs;

internal class ValidatorRegistry : ConcurrentDictionary<Type, Type>
{ }