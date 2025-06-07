using Microsoft.AspNetCore.Http;
using NetIdempo.Common.Store;
using NetIdempo.Implementations.Helpers.Headers;

namespace NetIdempo.Tests.Implementations.Helpers.Headers;

public class TestCacheHeaderCopier
{
    [Fact]
    public void CopyFromHttpContext_ShouldThrowArgumentNullException_WhenContextIsNull()
    {
        var copier = new CacheHeaderCopier();
        Assert.Throws<ArgumentNullException>(() => copier.CopyFromHttpContext(null!, new IdempotencyCacheEntry()));
    }
    
    [Fact]
    public void CopyFromHttpContext_ShouldThrowArgumentNullException_WhenEntryIsNull()
    {
        var copier = new CacheHeaderCopier();
        Assert.Throws<ArgumentNullException>(() => copier.CopyFromHttpContext(new DefaultHttpContext(), null!));
    }
    
    [Fact]
    public void CopyFromHttpContext__ShouldCopyHeaders_WhenHeadersArePresent()
    {
        var copier = new CacheHeaderCopier();
        var context = new DefaultHttpContext();
        context.Response.Headers["Test-Header"] = "TestValue";
        context.Response.Headers["Another-Header"] = "AnotherValue";
        
        var entry = new IdempotencyCacheEntry();
        copier.CopyFromHttpContext(context, entry);
        
        Assert.NotNull(entry.Headers);
        Assert.Equal("TestValue", entry.Headers["Test-Header"].FirstOrDefault());
        Assert.Equal("AnotherValue", entry.Headers["Another-Header"].FirstOrDefault());
    }
    
    [Fact]
    public void CopyToHttpContext_ShouldNotThrow_WhenHeadersAreNull()
    {
        var copier = new CacheHeaderCopier();
        var context = new DefaultHttpContext();
        var entry = new IdempotencyCacheEntry { Headers = null };
        
        // Should not throw
        copier.CopyToHttpContext(context, entry);
        
        Assert.Empty(context.Response.Headers);
    }
    
    [Fact]
    public void CopyToHttpContext_ShouldCopyHeaders_WhenHeadersAreNotNull()
    {
        var copier = new CacheHeaderCopier();
        var context = new DefaultHttpContext();
        var entry = new IdempotencyCacheEntry
        {
            Headers = new Dictionary<string, string[]>
            {
                { "Test-Header", new[] { "TestValue" } },
                { "Another-Header", new[] { "AnotherValue" } }
            }
        };
        
        copier.CopyToHttpContext(context, entry);
        
        Assert.Equal("TestValue", context.Response.Headers["Test-Header"].ToString());
        Assert.Equal("AnotherValue", context.Response.Headers["Another-Header"].ToString());
    }
}