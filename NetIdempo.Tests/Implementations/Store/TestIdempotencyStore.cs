using System.Text.Json;
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
    public async Task IdempotencyStore_ShouldThrowKeyNotFoundException_WhenKeyDoesNotExist()
    {
        // Arrange
        var cacheMock = new Mock<IDistributedCache>();
        var options = Options.Create(new NetIdempoOptions { IdempotencyKeyLifetime = 60 });
        cacheMock.Setup(c => c.GetAsync(It.IsAny<string>(), default));
        
        var store = new IdempotencyStore(cacheMock.Object, options);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            store.GetAsync("nonexistent-key"));
    }
    
    [Fact]
    public async Task IdempotencyStore_ShouldThrowJsonException_WhenDataCannotBeDeserialized()
    {
        // Arrange
        var cacheMock = new Mock<IDistributedCache>();
        var options = Options.Create(new NetIdempoOptions { IdempotencyKeyLifetime = 60 });
        cacheMock.Setup(c => c.GetAsync(It.IsAny<string>(), default))
            .ReturnsAsync(new byte[] { 0x01, 0x02, 0x03 }); 

        var store = new IdempotencyStore(cacheMock.Object, options);

        // Act & Assert
        await Assert.ThrowsAsync<JsonException>(() =>
            store.GetAsync("invalid-key"));
    }
    
    [Fact]
    public async Task IdempotencyStore_ShouldReturnJsonData_WhenKeyExists()
    {
        // Arrange
        var cacheMock = new Mock<IDistributedCache>();
        var options = Options.Create(new NetIdempoOptions { IdempotencyKeyLifetime = 60 });
        var entry = new IdempotencyCacheEntry
        {
            Key = "valid-key",
            CreatedAt = DateTime.UtcNow,
            Headers = null,
            IsCompleted = false,
            ResponseBody = null,
            StatusCode = 200
        };
        var jsonData = JsonSerializer.Serialize(entry);
        
        cacheMock.Setup(c => c.GetAsync("valid-key", default))
            .ReturnsAsync(System.Text.Encoding.UTF8.GetBytes(jsonData));

        var store = new IdempotencyStore(cacheMock.Object, options);

        // Act
        var result = await store.GetAsync("valid-key");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entry.Key, result.Key);
        Assert.Equal(entry.CreatedAt, result.CreatedAt);
        Assert.Equal(entry.IsCompleted, result.IsCompleted);
        Assert.Equal(entry.StatusCode, result.StatusCode);
        Assert.Null(result.Headers);
        Assert.Null(result.ResponseBody);
    }
}