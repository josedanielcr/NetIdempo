
using Microsoft.AspNetCore.Http;
using NetIdempo.Abstractions.Helpers.Body;
using NetIdempo.Abstractions.Helpers.Cache;
using NetIdempo.Abstractions.Helpers.Headers;
using NetIdempo.Common.Store;

namespace NetIdempo.Implementations.Helpers.Cache;

public class CacheResponseApplier(ICacheHeaderCopier cacheHeaderCopier,
    ICacheBodyCopier cacheBodyCopier) : ICacheResponseApplier
{
    public async Task ApplyToContextAsync(IdempotencyCacheEntry entry, HttpContext context)
    {
        context.Response.StatusCode = entry.StatusCode ?? StatusCodes.Status200OK;
        context.Response.Headers.Clear();
        cacheHeaderCopier.CopyToHttpContext(context, entry);
        await cacheBodyCopier.CopyToHttpContextAsync(entry, context);
    }
}