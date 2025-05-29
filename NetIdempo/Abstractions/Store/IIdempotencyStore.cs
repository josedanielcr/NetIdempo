using Microsoft.AspNetCore.Http;
using NetIdempo.Common.Store;

namespace NetIdempo.Abstractions.Store;

public interface IIdempotencyStore
{
    Task<IdempotencyCacheEntry?> GetAsync(string key);
    Task SetAsync(IdempotencyCacheEntry entry);
}