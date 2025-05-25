using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NetIdempo.Abstractions.Core;

namespace NetIdempo;

public class NetIdempoMiddleware(RequestDelegate next, IRequestReceiver receiver, ILogger<NetIdempoMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    { 
        context = await receiver.ReceiveRequestAsync(context);
        await next(context);
    }
}