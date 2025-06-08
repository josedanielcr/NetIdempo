using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using NetIdempo.Abstractions.Store;
using NetIdempo.Common;
using NetIdempo.Common.Store;

namespace NetIdempo.Implementations.Store;

public class IdempotencyStore(IDistributedCache cache, IOptions<NetIdempoOptions> options) : IIdempotencyStore
{
    public async Task<IdempotencyCacheEntry?> GetAsync(string key)
    {
        var rawResult = await cache.GetStringAsync(key);
        if (string.IsNullOrEmpty(rawResult)) return null;
        var result = JsonSerializer.Deserialize<IdempotencyCacheEntry>(rawResult!);
        if (result is null)
        {
            throw new JsonException($"Failed to deserialize IdempotencyCacheEntry from cache for key: {key}. The data may be corrupted or in an unexpected format.");
        }
        return result;
    }

    public async Task SetAsync(IdempotencyCacheEntry entry)
    {
        var json = JsonSerializer.Serialize(entry);
        await cache.SetStringAsync(entry.Key, json, options: 
            new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(options.Value.IdempotencyKeyLifetime)));
    }
}