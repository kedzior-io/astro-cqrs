using Serilog;

namespace Handlers.Abstractions;

public interface IHandlerContext
{
    ILogger Logger { get; }
}