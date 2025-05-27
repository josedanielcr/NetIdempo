using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NetIdempo.Abstractions.Helpers;
using NetIdempo.Common;

namespace NetIdempo.Implementations.Helpers;

public class ContextReader(IOptions<NetIdempoOptions> options) : IContextReader
{
    public bool IsKeyPresent(HttpContext? context)
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
}