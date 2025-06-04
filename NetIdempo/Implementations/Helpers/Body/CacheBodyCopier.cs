using Microsoft.AspNetCore.Http;
using NetIdempo.Abstractions.Helpers.Body;
using NetIdempo.Common.Store;

namespace NetIdempo.Implementations.Helpers.Body;

public class CacheBodyCopier : ICacheBodyCopier
{
    public async Task CopyFromContextAsync(HttpContext context, IdempotencyCacheEntry entry)
    {
        if (context.Response.Body is not { CanRead: true })
            return;

        var memoryStream = new MemoryStream();
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        await context.Response.Body.CopyToAsync(memoryStream);
        entry.ResponseBody = memoryStream.ToArray();
    }
}