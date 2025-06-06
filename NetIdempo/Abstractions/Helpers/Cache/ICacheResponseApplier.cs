using Microsoft.AspNetCore.Http;
using NetIdempo.Common.Store;

namespace NetIdempo.Abstractions.Helpers.Cache;

public interface ICacheResponseApplier
{
    Task ApplyToContextAsync(IdempotencyCacheEntry entry, HttpContext context);
}