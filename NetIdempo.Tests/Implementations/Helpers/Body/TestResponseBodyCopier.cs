using System.Net;
using Microsoft.AspNetCore.Http;
using NetIdempo.Implementations.Helpers.Body;

namespace NetIdempo.Tests.Implementations.Helpers.Body;

public class TestResponseBodyCopier
{
    [Fact]
    public async Task CopyToHttpContextAsync_ShouldCopyResponseBody()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("Test response body")
        };
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        var copier = new ResponseBodyCopier();

        // Act
        await copier.CopyToHttpContextAsync(response, context);

        // Assert
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(context.Response.Body);
        var result = await reader.ReadToEndAsync();
        
        Assert.Equal("Test response body", result);
    }

    [Fact]
    public async Task CopyToHttpContextAsync_ShouldHandleNullResponse()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        var copier = new ResponseBodyCopier();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => copier.CopyToHttpContextAsync(null, context));
    }
}