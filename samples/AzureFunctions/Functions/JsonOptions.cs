using System.Text.Json.Serialization;
using System.Text.Json;

namespace AstroCqrs.AzureFunctions;

public static class JsonOptions
{
    public static readonly JsonSerializerOptions Defaults = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        WriteIndented = true,
    };
}