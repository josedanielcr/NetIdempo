using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Moq;
using NetIdempo.Common;
using NetIdempo.Common.Store;
using NetIdempo.Implementations.Store;

namespace NetIdempo.Tests.Implementations.Store;

public class TestIdempotencyStore
{
    [Fact]
    public async Task GetAsync_ShouldReturnNull_WhenKeyDoesNotExist()
    {
        // Arrange
        var cache = new Mock<IDistributedCache>();
        cache.Setup(c => c.GetAsync(It.IsAny<string>(), default));
        
        var options = Options.Create(new NetIdempoOptions { IdempotencyKeyLifetime = 60 });
        var store = new IdempotencyStore(cache.Object, options);

        // Act
        var result = await store.GetAsync("nonexistent-key");

        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task GetAsync_ShouldReturnEntry_WhenKeyExists()
    {
        // Arrange
        var cache = new Mock<IDistributedCache>();
        var entry = new IdempotencyCacheEntry { Key = "existing-key" };
        var json = System.Text.Json.JsonSerializer.Serialize(entry);
        var bytes = Encoding.UTF8.GetBytes(json);
        cache.Setup(c => c.GetAsync("existing-key", default))
            .ReturnsAsync(bytes);
        
        
        var options = Options.Create(new NetIdempoOptions { IdempotencyKeyLifetime = 60 });
        var store = new IdempotencyStore(cache.Object, options);

        // Act
        var result = await store.GetAsync("existing-key");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entry.Key, result!.Key);
    }
    
    [Fact]
    public async Task SetAsync_ShouldStoreEntryInCache()
    {
        // Arrange
        var cache = new Mock<IDistributedCache>();
        var entry = new IdempotencyCacheEntry { Key = "new-key" };
        var json = System.Text.Json.JsonSerializer.Serialize(entry);
        var bytes = Encoding.UTF8.GetBytes(json);
        
        cache.Setup(c => c.SetAsync("new-key", It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), default))
            .Callback<string, byte[], DistributedCacheEntryOptions, CancellationToken>((key, value, options, token) =>
            {
                Assert.Equal(bytes, value);
            })
            .Returns(Task.CompletedTask);
        
        var options = Options.Create(new NetIdempoOptions { IdempotencyKeyLifetime = 60 });
        var store = new IdempotencyStore(cache.Object, options);

        // Act
        await store.SetAsync(entry);

        // Assert
        cache.Verify(c => c.SetAsync("new-key", It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), default), Times.Once);
    }
}