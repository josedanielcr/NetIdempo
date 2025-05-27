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
    
    public static void CopyHeaders(HttpRequestMessage request, HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(context);

        foreach (var header in context.Request.Headers)
        {
            if (!RestrictedHeaders.Contains(header.Key))
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
            }
        }
    }
}