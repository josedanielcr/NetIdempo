using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NetIdempo.Abstractions.Core;

namespace NetIdempo;

public class NetIdempoMiddleware(RequestDelegate next, IRequestHandler handler, ILogger<NetIdempoMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    { 
        logger.LogInformation("Handling request for {Path}", context.Request.Path);
        handler.HandleRequest(context);
        await next(context);
        logger.LogInformation("Handled request for {Path}", context.Request.Path);
    }
}