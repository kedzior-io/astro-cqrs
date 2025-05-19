namespace MinimalCqrs;

public static class ConsoleApp
{
    public static async Task<IHandlerResponse<TResponse>> Execute<TResponse>(IHandlerMessage<IHandlerResponse<TResponse>> request)
    {
        return await HandlerExtensions.ExecuteAsync(request, new CancellationToken());
    }
}