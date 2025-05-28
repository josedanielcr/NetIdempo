using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace NetIdempo.Implementations.Helpers;

public static class HeaderCopier
{
    private static readonly HashSet<string> RestrictedHeaders = new(StringComparer.OrdinalIgnoreCase)
    {
        "Host",
        "Content-Length",
        "Transfer-Encoding",
        "Connection",
        "Expect",
        "Keep-Alive"
    };
    
    public static void CopyContextHeadersToRequest(HttpContext context, HttpRequestMessage request)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(request);
        
        CopyContextHeaders(context, request);
    }

    public static void CopyResponseHeadersToContext(HttpResponseMessage response, HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(response);

        CopyResponseHeaders(response.Headers, context);
        CopyResponseHeaders(response.Content?.Headers, context);
    }
    
    private static void CopyResponseHeaders(HttpHeaders? headers, HttpContext context)
    {
        if (headers == null) return;

        foreach (var header in headers)
        {
            if (!RestrictedHeaders.Contains(header.Key))
            {
                context.Response.Headers[header.Key] = header.Value.ToArray();
            }
        }
    }

    private static void CopyContextHeaders(HttpContext context, HttpRequestMessage request)
    {
        foreach (var header in context.Request.Headers)
        {
            if (!RestrictedHeaders.Contains(header.Key))
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
            }
        }
    }
    
}