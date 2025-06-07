using System.Text;
using Microsoft.AspNetCore.Http;
using NetIdempo.Common.Store;
using NetIdempo.Implementations.Helpers.Body;
using NetIdempo.Implementations.Helpers.Cache;
using NetIdempo.Implementations.Helpers.Headers;

namespace NetIdempo.Tests.Implementations.Helpers.Cache;

public class TestCacheResponseApplier
{
    [Fact]
    public async Task ApplyToContextAsync_ShouldSetHeadersAndBodyToHttpContext()
    {
        // Arrange
        var cacheHeaderCopier = new CacheHeaderCopier();
        var cacheBodyCopier = new CacheBodyCopier();
        var applier = new CacheResponseApplier(cacheHeaderCopier, cacheBodyCopier);
        var entry = new IdempotencyCacheEntry
        {
            StatusCode = 200,
            Headers = new Dictionary<string, string[]> { { "Test-Header", ["hello"] } },
            ResponseBody = Encoding.UTF8.GetBytes("Test body content")
        };
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream(); 

        // Act
        await applier.ApplyToContextAsync(entry, context);

        // Assert
        Assert.Equal(200, context.Response.StatusCode);
        Assert.Equal(["hello"], context.Response.Headers["Test-Header"]!);
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(context.Response.Body);
        var bodyContent = await reader.ReadToEndAsync();
        Assert.Equal("Test body content", bodyContent);
    }
}