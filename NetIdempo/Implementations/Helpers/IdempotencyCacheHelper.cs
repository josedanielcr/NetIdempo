using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NetIdempo.Common;
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
    
    public static async Task CopyCachedResultToHttpContext(
        IdempotencyCacheEntry entry, HttpContext context)
    {
        context.Response.StatusCode = entry.StatusCode ?? StatusCodes.Status200OK;
        context.Response.Headers.Clear();
        CopyCachedHttpHeadersToHttpContext(entry,context);
        await CopyCachedBodyToHttpContext(entry, context);
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
    
    private static async Task CopyCachedBodyToHttpContext(
        IdempotencyCacheEntry entry, HttpContext context)
    {
        if (entry.ResponseBody == null) return;
        await context.Response.Body.WriteAsync(entry.ResponseBody, 0, entry.ResponseBody.Length);
    }
    
    public static async Task<IdempotencyCacheEntry> CopyHttpContextResultToCacheEntry(
        HttpContext context, string key)
    {
        var entry = new IdempotencyCacheEntry
        {
            Key = key,
            StatusCode = context.Response.StatusCode,
            IsCompleted = true
        };
        await BodyCopier.CopyContextBodyToCacheEntry(context, entry);
        HeaderCopier.CopyContextResultHeadersToCacheEntry(context, entry);
        return entry;
    }
}