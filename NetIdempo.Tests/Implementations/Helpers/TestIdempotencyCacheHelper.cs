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
    
    [Fact]
    public void CopyCachedResultToHttpContext_ShouldCopyHeadersToContext()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var entry = new IdempotencyCacheEntry
        {
            StatusCode = StatusCodes.Status200OK,
            Headers = new Dictionary<string, string[]>
            {
                { "Test-Header", new[] { "HeaderValue" } }
            },
            ResponseBody = null
        };

        // Act
        IdempotencyCacheHelper.CopyCachedResultToHttpContext(entry, context);

        // Assert
        Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
        Assert.True(context.Response.Headers.ContainsKey("Test-Header"));
        Assert.Equal("HeaderValue", context.Response.Headers["Test-Header"]);
    }
    
    [Fact]
    public void CopyCachedResultToHttpContext_ShouldCopyBodyToContext()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var memoryStream = new MemoryStream();
        context.Response.Body = memoryStream;

        var responseBody = System.Text.Encoding.UTF8.GetBytes("Test response body");
        var entry = new IdempotencyCacheEntry
        {
            StatusCode = StatusCodes.Status200OK,
            Headers = null,
            ResponseBody = responseBody
        };

        // Act
        IdempotencyCacheHelper.CopyCachedResultToHttpContext(entry, context);

        // Assert
        Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(context.Response.Body);
        var bodyContent = reader.ReadToEnd();

        Assert.Equal("Test response body", bodyContent);
    }
    
    [Fact]
    public void CopyCachedResultToHttpContext_ShouldHandleNullHeadersAndBody()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var entry = new IdempotencyCacheEntry
        {
            StatusCode = StatusCodes.Status200OK,
            Headers = null,
            ResponseBody = null
        };

        // Act
        IdempotencyCacheHelper.CopyCachedResultToHttpContext(entry, context);

        // Assert
        Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
        Assert.Empty(context.Response.Headers);
        Assert.Equal(0, context.Response.Body.Length);
    }

}