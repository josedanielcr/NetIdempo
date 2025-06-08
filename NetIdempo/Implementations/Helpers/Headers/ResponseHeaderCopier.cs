using Microsoft.AspNetCore.Http;
using NetIdempo.Abstractions.Helpers.Headers;
using NetIdempo.Common.Core;

namespace NetIdempo.Implementations.Helpers.Headers;

public class ResponseHeaderCopier : IResponseHeaderCopier
{
    public void CopyToHttpContext(HttpResponseMessage response, HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(response);

        foreach (var header in response.Headers)
        {
            if (!HttpHeaderConstants.RestrictedHeaders.Contains(header.Key))
            {
                context.Response.Headers[header.Key] = header.Value.ToArray();
            }
        }
        
        foreach (var header in response.Content.Headers)
        {
            if (!HttpHeaderConstants.RestrictedHeaders.Contains(header.Key))
            {
                context.Response.Headers[header.Key] = header.Value.ToArray();
            }
        }
    }
}