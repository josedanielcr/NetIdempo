using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using NetIdempo.Abstractions.Helpers.Body;

namespace NetIdempo.Implementations.Helpers.Body;

public class RequestBodyCopier : IRequestBodyCopier
{
    public async Task<HttpRequestMessage> CopyFromHttpContextAsync(HttpRequestMessage request, HttpContext context)
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
}