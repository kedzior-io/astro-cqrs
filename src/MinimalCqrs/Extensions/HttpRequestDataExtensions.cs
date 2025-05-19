using Microsoft.Azure.Functions.Worker.Http;
using System.Collections.Specialized;
using System.Web;

namespace MinimalCqrs.Extensions;

public static class HttpRequestDataExtensions
{
    public static async Task<NameValueCollection> FormData(this HttpRequestData request)
    {
        var body = await request.ReadAsStringAsync();

        return string.IsNullOrWhiteSpace(body) ? new NameValueCollection() : HttpUtility.ParseQueryString(body);
    }

    public static async Task<string> BodyAsync(this HttpRequestData request)
    {
        return await new StreamReader(request.Body).ReadToEndAsync();
    }
}