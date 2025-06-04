using Microsoft.AspNetCore.Http;
using NetIdempo.Common.Store;

namespace NetIdempo.Abstractions.Helpers.Body;

public interface ICacheBodyCopier
{
    Task CopyFromContextAsync(HttpContext context, IdempotencyCacheEntry entry);
}