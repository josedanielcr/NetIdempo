using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using NetIdempo.Common.Store;

namespace NetIdempo.Implementations.Helpers;

public static class BodyCopier
{
    public static async Task<HttpRequestMessage> CopyContextBodyToRequest(HttpRequestMessage request, HttpContext context)
    {
        if (context.Request.Body is not { CanRead: true })
            return request;

        var buffer = new MemoryStream();
        await context.Request.Body.CopyToAsync(buffer);
        buffer.Position = 0;

        var content = new StreamContent(buffer);

        if (!string.IsNullOrEmpty(context.Request.ContentType))
        {
            content.Headers.ContentType = new MediaTypeHeaderValue(context.Request.ContentType);
        }

        request.Content = content;
        return request;
    }
    
    public static async Task CopyResponseBodyToContext(HttpResponseMessage response, HttpContext context)
    {
        var memoryStream = new MemoryStream();
        await response.Content.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        await memoryStream.CopyToAsync(context.Response.Body);
        memoryStream.Position = 0;
        response.Content = new StreamContent(memoryStream);
    }
    
    public static async Task CopyContextBodyToCacheEntry(HttpContext context, IdempotencyCacheEntry entry)
    {
        if (context.Response.Body is not { CanRead: true })
            return;

        var memoryStream = new MemoryStream();
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        await context.Response.Body.CopyToAsync(memoryStream);
        entry.ResponseBody = memoryStream.ToArray();
    }
}