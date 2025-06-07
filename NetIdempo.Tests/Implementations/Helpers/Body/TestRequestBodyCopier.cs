using Microsoft.AspNetCore.Http;
using NetIdempo.Implementations.Helpers.Body;

namespace NetIdempo.Tests.Implementations.Helpers.Body;

public class TestRequestBodyCopier
{
    [Fact]
    public async Task CopyFromHttpContextAsync_ShouldReturnRequest_WhenBodyIsNotReadable()
    {
        // Arrange
        var request = new HttpRequestMessage();
        var context = new DefaultHttpContext();
        context.Request.Body = Stream.Null;

        var copier = new RequestBodyCopier();

        // Act
        var result = await copier.CopyFromHttpContextAsync(request, context);

        // Assert
        Assert.Equal(request, result);
    }
    
    [Fact]
    public async Task CopyFromHttpContextAsync_ShouldCopyContextBodyToRequest_WhenBodyIsReadable()
    {
        // Arrange
        var request = new HttpRequestMessage();
        var context = new DefaultHttpContext();
        var bodyContent = "Test content";
        context.Request.Body = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(bodyContent));
        context.Request.ContentType = "text/plain";

        var copier = new RequestBodyCopier();

        // Act
        var result = await copier.CopyFromHttpContextAsync(request, context);

        // Assert
        Assert.NotNull(result.Content);
        var contentString = await result.Content.ReadAsStringAsync();
        Assert.Equal(bodyContent, contentString);
        Assert.Equal("text/plain", result.Content.Headers.ContentType?.MediaType);
    }
}