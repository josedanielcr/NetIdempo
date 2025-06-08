using Microsoft.AspNetCore.Http;
using NetIdempo.Implementations.Helpers.Headers;

namespace NetIdempo.Tests.Implementations.Helpers.Headers;

public class TestRequestHeaderCopier
{
    [Fact]
    public void CopyFromHttpContext_ShouldThrowArgumentNullException_WhenHttpContextIsNull()
    {
        // Arrange
        var request = new HttpRequestMessage();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new RequestHeaderCopier().CopyFromHttpContext(request, null!));
    }
    
    [Fact]
    public void CopyFromHttpContext_ShouldThrowArgumentNullException_WhenHttpRequestMessageIsNull()
    {
        // Arrange
        var context = new DefaultHttpContext();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new RequestHeaderCopier().CopyFromHttpContext(null!, context));
    }
    
    [Fact]
    public void CopyFromHttpContext_ShouldCopyHeaders_WhenHeadersArePresent()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Headers["Test-Header"] = "TestValue";
        var request = new HttpRequestMessage();

        // Act
        new RequestHeaderCopier().CopyFromHttpContext(request, context);

        // Assert
        Assert.True(request.Headers.Contains("Test-Header"));
        Assert.Equal("TestValue", request.Headers.GetValues("Test-Header").First());
    }
}