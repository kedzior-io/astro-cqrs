using Microsoft.AspNetCore.Http;

namespace AstroCqrs;

internal static class CtxKey
{
    //values are strings to avoid boxing when doing dictionary lookups in HttpContext.Items
    internal const string ResponseStarted = "0";

    internal const string ValidationFailures = "1";
    internal const string ProcessorState = "2";
}