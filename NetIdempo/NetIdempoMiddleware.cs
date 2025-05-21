using Microsoft.AspNetCore.Http;
namespace NetIdempo;

public class NetIdempoMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    { 
        Console.WriteLine($"[NetIdempo] Request started: {context.Request.Method} {context.Request.Path}"); 
        await next(context);
        Console.WriteLine($"[NetIdempo] Request ended: {context.Request.Method} {context.Request.Path}");
    }
}