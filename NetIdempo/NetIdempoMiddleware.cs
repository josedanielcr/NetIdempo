using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetIdempo.Abstractions.Core;

namespace NetIdempo;

public class NetIdempoMiddleware(RequestDelegate next, ILogger<NetIdempoMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    { 
        var receiver = context.RequestServices.GetRequiredService<IRequestReceiver>();
        await receiver.ReceiveRequestAsync(context);
        await next(context);
    }
}