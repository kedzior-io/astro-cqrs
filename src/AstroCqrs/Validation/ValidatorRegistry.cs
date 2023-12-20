using System.Collections.Concurrent;

namespace AstroCqrs;

internal class ValidatorRegistry : ConcurrentDictionary<Type, Type>
{ }