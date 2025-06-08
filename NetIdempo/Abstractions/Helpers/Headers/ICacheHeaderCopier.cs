using Microsoft.AspNetCore.Http;
using NetIdempo.Common.Store;

namespace NetIdempo.Abstractions.Helpers.Headers;

public interface ICacheHeaderCopier
{
    void CopyFromHttpContext(HttpContext context, IdempotencyCacheEntry entry);
    void CopyToHttpContext(HttpContext context, IdempotencyCacheEntry entry);
}