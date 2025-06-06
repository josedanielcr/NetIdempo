using Microsoft.AspNetCore.Http;
using NetIdempo.Abstractions.Helpers.Headers;
using NetIdempo.Common.Core;

namespace NetIdempo.Implementations.Helpers.Headers;

public class RequestHeaderCopier : IRequestHeaderCopier
{
    public void CopyFromHttpContext(HttpRequestMessage request, HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(request);
        
        foreach (var header in context.Request.Headers)
        {
            if (!HttpHeaderConstants.RestrictedHeaders.Contains(header.Key))
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
            }
        }
    }
}