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
    
    public static void CopyCachedResultToHttpContext(
        IdempotencyCacheEntry entry, HttpContext context)
    {
        context.Response.StatusCode = entry.StatusCode ?? StatusCodes.Status200OK;
        context.Response.Headers.Clear();
        CopyCachedHttpHeadersToHttpContext(entry,context);
        CopyCachedBodyToHttpContext(entry, context);
    }
    
    private static void CopyCachedHttpHeadersToHttpContext(
        IdempotencyCacheEntry entry, HttpContext context)
    {
        if (entry.Headers == null) return;
        foreach (var header in entry.Headers)
        {
            context.Response.Headers[header.Key] = header.Value;
        }
    }
    
    private static void CopyCachedBodyToHttpContext(
        IdempotencyCacheEntry entry, HttpContext context)
    {
        if (entry.ResponseBody == null) return;
        context.Response.Body.Write(entry.ResponseBody, 0, entry.ResponseBody.Length);
    }
}