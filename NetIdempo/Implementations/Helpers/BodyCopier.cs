using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace NetIdempo.Implementations.Helpers;

public static class BodyCopier
{
    public static HttpRequestMessage CopyContextBodyToRequest(HttpRequestMessage request, HttpContext context)
    {
        if (context.Request.Body is not { CanRead: true })
            return request;

        var buffer = new MemoryStream();
        context.Request.Body.CopyTo(buffer);
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
        await response.Content.CopyToAsync(context.Response.Body, context.RequestAborted);
    }
}