using Microsoft.AspNetCore.Http;
using NetIdempo.Common.Store;

namespace NetIdempo.Abstractions.Helpers.Body;

public interface ICacheBodyCopier
{
    Task CopyFromHttpContextAsync(HttpContext context, IdempotencyCacheEntry entry);
    Task CopyToHttpContextAsync(IdempotencyCacheEntry entry, HttpContext context);
}