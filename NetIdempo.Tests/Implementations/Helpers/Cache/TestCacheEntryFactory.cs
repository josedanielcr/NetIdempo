using Microsoft.AspNetCore.Http;
using Moq;
using NetIdempo.Abstractions.Helpers.Body;
using NetIdempo.Abstractions.Helpers.Headers;
using NetIdempo.Implementations.Helpers.Body;
using NetIdempo.Implementations.Helpers.Cache;
using NetIdempo.Implementations.Helpers.Headers;

namespace NetIdempo.Tests.Implementations.Helpers.Cache;

public class TestCacheEntryFactory
{
    [Fact]
    public void CreateEmpty_ShouldReturnNullIfKeyIsEmpty()
    {
        // Arrange
        var factory = new CacheEntryFactory(null, null);

        // Act
        var result = factory.CreateEmpty("");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void CreateEmpty_ShouldReturnEntryWithGivenKey()
    {
        // Arrange
        var factory = new CacheEntryFactory(null, null);
        var key = "test-key";
        
        // Act
        var result = factory.CreateEmpty(key);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(key, result.Key);
        Assert.NotEqual(DateTime.MinValue, result.CreatedAt);
        Assert.False(result.IsCompleted);
        Assert.Null(result.ResponseBody);
        Assert.Null(result.Headers);
        Assert.Null(result.StatusCode);
    }
    
    [Fact]
    public async Task CreateFromHttpContextAsync_ShouldReturnEntryWithStatusCodeAndHeaders()
    {
        // Arrange
        var cacheHeaderCopier = new CacheHeaderCopier();
        var cacheBodyCopier = new CacheBodyCopier();
        var factory = new CacheEntryFactory(cacheHeaderCopier, cacheBodyCopier);
        var context = new DefaultHttpContext();
        context.Response.StatusCode = 200;
        context.Response.Headers["Test-Header"] = "HeaderValue";
        var key = "test-key";

        // Act
        var result = await factory.CreateFromHttpContextAsync(context, key);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(key, result.Key);
        Assert.Equal(200, result.StatusCode);
        Assert.True(result.IsCompleted);
        Assert.NotNull(result.Headers);
        Assert.Contains("Test-Header", result.Headers.Keys);
    }
}