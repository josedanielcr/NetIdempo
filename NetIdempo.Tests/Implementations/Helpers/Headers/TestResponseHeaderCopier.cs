using Microsoft.AspNetCore.Http;
using NetIdempo.Implementations.Helpers.Headers;

namespace NetIdempo.Tests.Implementations.Helpers.Headers;

public class TestResponseHeaderCopier
{
    [Fact]
    public void CopyToHttpContext_ShouldThrowArgumentNullException_WhenContextIsNull()
    {
        // Arrange
        var response = new HttpResponseMessage();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ResponseHeaderCopier().CopyToHttpContext(response, null!));
    }
    
    [Fact]
    public void CopyToHttpContext_ShouldThrowArgumentNullException_WhenResponseIsNull()
    {
        // Arrange
        var context = new DefaultHttpContext();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ResponseHeaderCopier().CopyToHttpContext(null!, context));
    }
    
    [Fact]
    public void CopyToHttpContext_ShouldCopyHeaders_WhenHeadersArePresent()
    {
        // Arrange
        var response = new HttpResponseMessage();
        response.Headers.Add("Test-Header", "TestValue");
        response.Content.Headers.Add("Content-Test-Header", "ContentTestValue");

        var context = new DefaultHttpContext();

        // Act
        new ResponseHeaderCopier().CopyToHttpContext(response, context);

        // Assert
        Assert.True(context.Response.Headers.ContainsKey("Test-Header"));
        Assert.Equal("TestValue", context.Response.Headers["Test-Header"].ToString());
        
        Assert.True(context.Response.Headers.ContainsKey("Content-Test-Header"));
        Assert.Equal("ContentTestValue", context.Response.Headers["Content-Test-Header"].ToString());
    }
}