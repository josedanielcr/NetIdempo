using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using NetIdempo.Abstractions.Helpers;
using NetIdempo.Common;

namespace NetIdempo.Implementations.Helpers;

public class ContextReader(IOptions<NetIdempoOptions> options) : IContextReader
{
    public bool IsIdempotencyKeyPresent(HttpContext? context)
    {
        return context != null && context.Request.Headers.ContainsKey(options.Value.IdempotencyKeyHeader);
    }

    public string GetServiceByPath(HttpContext context)
    {
        if (!context.Request.Path.HasValue) return string.Empty;
        
        var path = context.Request.Path.Value;
        if(path == null) return string.Empty;
        foreach (var service in options.Value.Services.Where(service => path.Contains(service.Value.Key)))
        {
            return service.Key;
        }
        return string.Empty;
    }
    
    public StringValues GetKeyFromHttpRequest(HttpContext context)
    {
        var key = context.Request.Headers[options.Value.IdempotencyKeyHeader];
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException("Idempotency key is missing in the request headers.");
        }
        return key;
    }
}