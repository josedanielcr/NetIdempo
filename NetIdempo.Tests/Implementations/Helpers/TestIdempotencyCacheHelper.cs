using Microsoft.AspNetCore.Http;
using NetIdempo.Common.Store;
using NetIdempo.Implementations.Helpers;

namespace NetIdempo.Tests.Implementations.Helpers;

public class TestIdempotencyCacheHelper
{
    [Fact]
    public void CreateEmptyCacheEntry_ShouldReturnEmptyEntry()
    {
        // Arrange
        var key = "test-key";
        var expectedEntry = new IdempotencyCacheEntry
        {
            Key = key,
            CreatedAt = DateTime.UtcNow,
            IsCompleted = false,
            ResponseBody = null,
            Headers = null,
            StatusCode = null
        };

        // Act
        var entry = IdempotencyCacheHelper.CreateEmptyCacheEntry(key);

        // Assert
        Assert.Equal(expectedEntry.Key, entry.Key);
        Assert.Equal(expectedEntry.IsCompleted, entry.IsCompleted);
        Assert.Null(entry.ResponseBody);
        Assert.Null(entry.Headers);
        Assert.Null(entry.StatusCode);
        Assert.Equal(expectedEntry.StatusCode, entry.StatusCode);
    }
}