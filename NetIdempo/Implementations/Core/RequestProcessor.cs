using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using NetIdempo.Abstractions.Core;
using NetIdempo.Abstractions.Helpers.Cache;
using NetIdempo.Abstractions.Helpers.Config;
using NetIdempo.Abstractions.Helpers.HttpUtils;
using NetIdempo.Abstractions.Services;
using NetIdempo.Abstractions.Store;
using NetIdempo.Common;
using NetIdempo.Common.Store;

namespace NetIdempo.Implementations.Core;

public class RequestProcessor(IContextReader contextReader, 
    IRequestForwarder forwarder, IIdempotencyStore idempotencyStore,
    ICacheResponseApplier cacheResponseApplier,
    ICacheEntryFactory cacheEntryFactory,
    IOptionsReader optionsReader) : IRequestProcessor
{
    public async Task ProcessRequestAsync(HttpContext context)
    {
        if (await ProcessNonIdempotentRequestAsync(context)) return;
        
        var (key, entry) = await GetCachedIdempotentEntry(context);
        
        if (await ProcessNewIdempotentRequestAsync(context, entry, key)) return;
        
        if (ProcessPendingIdempotentRequest(context, entry!, key)) return;
        await cacheResponseApplier.ApplyToContextAsync(entry!, context);
    }

    private async Task<bool> ProcessNewIdempotentRequestAsync(HttpContext context, IdempotencyCacheEntry? entry, StringValues key)
    {
        if (entry != null) return false;
        await ForwardNewRequest(context, key);
        return true;
    }

    private async Task<bool> ProcessNonIdempotentRequestAsync(HttpContext context)
    {
        if (contextReader.IsIdempotencyKeyPresent(context)) return false;
        await forwarder.ForwardRequestAsync(context);
        return true;
    }

    private async Task<(StringValues key, IdempotencyCacheEntry? entry)> GetCachedIdempotentEntry(HttpContext context)
    {
        var key = contextReader.GetKeyFromHttpRequest(context);
        var entry = await idempotencyStore.GetAsync(key!);
        return (key, entry);
    }

    private bool ProcessPendingIdempotentRequest(HttpContext context, IdempotencyCacheEntry entry, StringValues key)
    {
        if (entry.IsCompleted) return false;
        context.Response.StatusCode = StatusCodes.Status202Accepted;
        SetIdempotencyKeyHeaderToHttpContext(context, key);
        return true;
    }

    private async Task ForwardNewRequest(HttpContext context, StringValues key)
    {
        SetIdempotencyKeyHeaderToHttpContext(context, key);
        using var memoryStream = new MemoryStream();
        var originalBodyStream = RedirectResponseBodyToMemory(context, memoryStream);

        var cacheEntry = await CacheEmptyIdempotencyRequest(key);
        
        await forwarder.ForwardRequestAsync(context);
        
        memoryStream.Position = 0;
        await memoryStream.CopyToAsync(originalBodyStream);
        memoryStream.Position = 0;
        
        await CacheHttpContextResponse(context, cacheEntry);
        context.Response.Body = originalBodyStream;
    }

    private async Task CacheHttpContextResponse(HttpContext context, IdempotencyCacheEntry cacheEntry)
    {
        var entry = await cacheEntryFactory.CreateFromHttpContextAsync(context, cacheEntry.Key);
        await idempotencyStore.SetAsync(entry);
    }

    private async Task<IdempotencyCacheEntry> CacheEmptyIdempotencyRequest(StringValues key)
    {
        var cacheEntry = cacheEntryFactory.CreateEmpty(key!);
        await idempotencyStore.SetAsync(cacheEntry);
        return cacheEntry;
    }

    private void SetIdempotencyKeyHeaderToHttpContext(HttpContext context, StringValues key)
    {
        context.Response.Headers[optionsReader.GetIdempotencyKeyHeader()] = key;
    }
    
    private static Stream RedirectResponseBodyToMemory(HttpContext context, MemoryStream memoryStream)
    {
        var original = context.Response.Body;
        context.Response.Body = memoryStream;
        return original;
    }
}