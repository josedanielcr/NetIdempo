using Microsoft.AspNetCore.Http;
using NetIdempo.Common.Store;

namespace NetIdempo.Abstractions.Helpers.Cache;

public interface ICacheEntryFactory
{
    IdempotencyCacheEntry CreateEmpty(string key);
    Task<IdempotencyCacheEntry> CreateFromHttpContextAsync(HttpContext context, string key);
}