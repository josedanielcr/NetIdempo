using Microsoft.AspNetCore.Http;
using NetIdempo.Common.Store;
using NetIdempo.Implementations.Helpers.Body;

namespace NetIdempo.Tests.Implementations.Helpers.Body;

public class TestCacheBodyCopier
{
    [Fact]
    public async Task CopyFromHttpContextAsync_ShouldNotThrow_WhenResponseBodyIsNull()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var entry = new IdempotencyCacheEntry();

        var copier = new CacheBodyCopier();

        // Act & Assert
        await copier.CopyFromHttpContextAsync(context, entry);
    }
    
    [Fact]
    public async Task CopyFromHttpContextAsync_ShouldCopyResponseBody_ToEntry()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var responseBody = "Test response body";
        context.Response.Body = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(responseBody));
        var entry = new IdempotencyCacheEntry();

        var copier = new CacheBodyCopier();

        // Act
        await copier.CopyFromHttpContextAsync(context, entry);

        // Assert
        Assert.NotNull(entry.ResponseBody);
        Assert.Equal(responseBody, System.Text.Encoding.UTF8.GetString(entry.ResponseBody));
    }
    
    [Fact]
    public async Task CopyToHttpContextAsync_ShouldNotThrow_WhenResponseBodyIsNull()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var entry = new IdempotencyCacheEntry();

        var copier = new CacheBodyCopier();

        // Act & Assert
        await copier.CopyToHttpContextAsync(entry, context);
    }
    
    [Fact]
    public async Task CopyToHttpContextAsync_ShouldWriteResponseBody_FromEntry()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var responseBody = "Test response body";
        var entry = new IdempotencyCacheEntry
        {
            ResponseBody = System.Text.Encoding.UTF8.GetBytes(responseBody)
        };

        context.Response.Body = new MemoryStream();

        var copier = new CacheBodyCopier();

        // Act
        await copier.CopyToHttpContextAsync(entry, context);

        // Assert
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(context.Response.Body);
        var result = await reader.ReadToEndAsync();
        
        Assert.Equal(responseBody, result);
    }
}