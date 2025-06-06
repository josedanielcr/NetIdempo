using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http;
using NetIdempo.Abstractions.Helpers.Config;
using NetIdempo.Abstractions.Helpers.HttpUtils;

namespace NetIdempo.Implementations.Helpers.HttpUtils;

public class ContextReader(IOptionsReader optionsReader) : IContextReader
{
    public bool IsIdempotencyKeyPresent(HttpContext? context)
    {
        return context != null && context.Request.Headers.ContainsKey(optionsReader.GetIdempotencyKeyHeader());
    }
    
    public string GetDestinationServiceFromHttpContext(HttpContext context)
    {
        var serviceKey = GetServiceByPath(context);
        if (string.IsNullOrEmpty(serviceKey))
        {
            throw new ArgumentException("Service key not found for the given request path.");
        }
        return serviceKey;
    }

    private string GetServiceByPath(HttpContext context)
    {
        if (!context.Request.Path.HasValue) return string.Empty;
        var path = context.Request.Path.Value;
        return path == null 
            ? string.Empty 
            : optionsReader.GetServiceKeyByIncomingRequestPath(path);
    }
    
    public StringValues GetKeyFromHttpRequest(HttpContext context)
    {
        var key = context.Request.Headers[optionsReader.GetIdempotencyKeyHeader()];
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException("Idempotency key is missing in the request headers.");
        }
        return key;
    }

    public string GetDestinationFinalUrlFromContext(HttpContext context, string servicePrexif)
    {
        var index = context.Request.Path.ToString().IndexOf(servicePrexif, StringComparison.Ordinal);
        var remainingPath = context.Request.Path.ToString().Substring(index + servicePrexif.Length).Trim();
        if (string.IsNullOrEmpty(remainingPath))
        {
            throw new ArgumentException($"No remaining path found for service key '{servicePrexif}'.");
        }
        return remainingPath;
    }
}