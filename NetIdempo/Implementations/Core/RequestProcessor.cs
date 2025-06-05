using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using NetIdempo.Abstractions.Core;
using NetIdempo.Abstractions.Helpers.Cache;
using NetIdempo.Abstractions.Helpers.HttpUtils;
using NetIdempo.Abstractions.Services;
using NetIdempo.Abstractions.Store;
using NetIdempo.Common;
using NetIdempo.Common.Store;
using NetIdempo.Implementations.Helpers;

namespace NetIdempo.Implementations.Core;

public class RequestProcessor(IOptions<NetIdempoOptions> options, IContextReader contextReader, 
    IRequestForwarder forwarder, IIdempotencyStore idempotencyStore,
    ICacheResponseApplier cacheResponseApplier,
    ICacheEntryFactory cacheEntryFactory) : IRequestProcessor
{
    public async Task ProcessRequestAsync(HttpContext context)
    {
        if (!contextReader.IsIdempotencyKeyPresent(context))
        {
            await forwarder.ForwardRequestAsync(context);
            return;
        }

        var key = contextReader.GetKeyFromHttpRequest(context);
        var entry = await idempotencyStore.GetAsync(key!);

        if (entry == null)
        {
            await ForwardNewRequest(context, key);
            return;
        }

        if (HandlePendingIdempotentRequest(context, entry, key)) return;
        await cacheResponseApplier.ApplyToContextAsync(entry, context);
    }

    private bool HandlePendingIdempotentRequest(HttpContext context, IdempotencyCacheEntry entry, StringValues key)
    {
        if (entry.IsCompleted) return false;
        context.Response.StatusCode = StatusCodes.Status202Accepted;
        context.Response.Headers[options.Value.IdempotencyKeyHeader] = key;
        return true;
    }

    private async Task ForwardNewRequest(HttpContext context, StringValues key)
    {
        context.Response.Headers[options.Value.IdempotencyKeyHeader] = key;
        var originalBodyStream = context.Response.Body;
        using var memoryStream = new MemoryStream();
        context.Response.Body = memoryStream;
        
        var cacheEntry = cacheEntryFactory.CreateEmpty(key!);
        await idempotencyStore.SetAsync(cacheEntry);
        
        await forwarder.ForwardRequestAsync(context);
        
        memoryStream.Position = 0;
        await memoryStream.CopyToAsync(originalBodyStream);
        memoryStream.Position = 0;
        
        var entry = await cacheEntryFactory.CreateFromHttpContextAsync(context, cacheEntry.Key);
        await idempotencyStore.SetAsync(entry);
        context.Response.Body = originalBodyStream;
    }
}