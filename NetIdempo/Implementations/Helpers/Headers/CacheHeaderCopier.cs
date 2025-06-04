using Microsoft.AspNetCore.Http;
using NetIdempo.Abstractions.Helpers.Headers;
using NetIdempo.Common.Core;
using NetIdempo.Common.Store;

namespace NetIdempo.Implementations.Helpers.Headers;

public class CacheHeaderCopier : ICacheHeaderCopier
{
    public void CopyContextResultHeadersToCacheEntry(HttpContext context, IdempotencyCacheEntry entry)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(entry);

        entry.Headers = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);
        
        foreach (var header in context.Response.Headers)
        {
            if (!HttpHeaderConstants.RestrictedHeaders.Contains(header.Key))
            {
                entry.Headers[header.Key] = header.Value!;
            }
        }
    }
}