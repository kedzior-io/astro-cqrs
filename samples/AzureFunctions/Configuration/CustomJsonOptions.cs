using System.Text.Json.Serialization;
using System.Text.Json;

namespace AzureFunctions.Configuration;

public static class CustomJsonOptions
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