using Microsoft.AspNetCore.Http;
using NetIdempo.Abstractions.Helpers.Body;
using NetIdempo.Abstractions.Helpers.Cache;
using NetIdempo.Abstractions.Helpers.Headers;
using NetIdempo.Common.Store;

namespace NetIdempo.Implementations.Helpers.Cache;

public class CacheEntryFactory(ICacheHeaderCopier cacheHeaderCopier,
    ICacheBodyCopier cacheBodyCopier) : ICacheEntryFactory
{
    public IdempotencyCacheEntry CreateEmpty(string key)
    {
        if (string.IsNullOrWhiteSpace(key)) return null!;
        return new IdempotencyCacheEntry
        {
            Key = key,
            CreatedAt = DateTime.UtcNow,
            IsCompleted = false,
            ResponseBody = null,
            Headers = null,
            StatusCode = null
        };
    }

    public async Task<IdempotencyCacheEntry> CreateFromHttpContextAsync(HttpContext context, string key)
    {
        var entry = new IdempotencyCacheEntry
        {
            Key = key,
            StatusCode = context.Response.StatusCode,
            IsCompleted = true
        };
        cacheHeaderCopier.CopyFromHttpContext(context, entry);
        await cacheBodyCopier.CopyFromHttpContextAsync(context, entry);
        return entry;
    }
}