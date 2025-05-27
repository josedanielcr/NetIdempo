using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NetIdempo.Abstractions.Core;
using NetIdempo.Abstractions.Helpers;
using NetIdempo.Common;
using NetIdempo.Implementations.Store;

namespace NetIdempo.Implementations.Core;

public class RequestProcessor(IOptions<NetIdempoOptions> options, IContextReader contextReader, 
    IRequestForwarder forwarder, IdempotencyStore idempotencyStore) : IRequestProcessor
{
    public async Task<HttpContext> ProcessRequestAsync(HttpContext context)
    {
        if (!contextReader.IsKeyPresent(context))
        {
            // If not present, call the forwarding processor
            await forwarder.ForwardRequestAsync(context);
        }
        // try to get the idempotency value from cache
        return context;
    }
}