using Microsoft.AspNetCore.Http;
using NetIdempo.Common.Store;

namespace NetIdempo.Abstractions.Helpers.Headers;

public interface ICacheHeaderCopier
{
    void CopyContextResultHeadersToCacheEntry(HttpContext context, IdempotencyCacheEntry entry);
}