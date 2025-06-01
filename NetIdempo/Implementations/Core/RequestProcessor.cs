using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using NetIdempo.Abstractions.Core;
using NetIdempo.Abstractions.Helpers;
using NetIdempo.Abstractions.Store;
using NetIdempo.Common;
using NetIdempo.Common.Store;
using NetIdempo.Implementations.Helpers;

namespace NetIdempo.Implementations.Core;

public class RequestProcessor(IOptions<NetIdempoOptions> options, IContextReader contextReader, 
    IRequestForwarder forwarder, IIdempotencyStore idempotencyStore) : IRequestProcessor
{
    public async Task<HttpContext> ProcessRequestAsync(HttpContext context)
    {
        if (!contextReader.IsIdempotencyKeyPresent(context))
        {
            return await forwarder.ForwardRequestAsync(context);
        }

        var key = contextReader.GetKeyFromHttpRequest(context);
        var entry = await idempotencyStore.GetAsync(key!);

        if (entry == null) return await ForwardNewRequest(context, key);
        if (HandlePendingIdempotentRequest(context, entry, key)) return context;
        
        IdempotencyCacheHelper.CopyCachedResultToHttpContext(entry, context);
        return context;
    }

    private bool HandlePendingIdempotentRequest(HttpContext context, IdempotencyCacheEntry entry, StringValues key)
    {
        if (entry.IsCompleted) return false;
        context.Response.StatusCode = StatusCodes.Status202Accepted;
        context.Response.Headers[options.Value.IdempotencyKeyHeader] = key;
        return true;
    }

    private async Task<HttpContext> ForwardNewRequest(HttpContext context, StringValues key)
    {
        var cacheEntry = IdempotencyCacheHelper.CreateEmptyCacheEntry(key!);
        await idempotencyStore.SetAsync(cacheEntry);
        return await forwarder.ForwardRequestAsync(context);
    }
}