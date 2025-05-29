using Microsoft.AspNetCore.Http;
using NetIdempo.Common.Store;

namespace NetIdempo.Implementations.Helpers;

public static class IdempotencyCacheHelper
{
    public static IdempotencyCacheEntry CreateEmptyCacheEntry(string key)
    {
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
    
    public static HttpContext CopyCachedResultToHttpContext(
        IdempotencyCacheEntry entry, HttpContext context)
    {
        context.Response.StatusCode = entry.StatusCode ?? StatusCodes.Status200OK;
        context.Response.Headers.Clear();

        if (entry.ResponseBody != null)
        {
            context.Response.Body.Write(entry.ResponseBody, 0, entry.ResponseBody.Length);
        }

        return context;
    }
}