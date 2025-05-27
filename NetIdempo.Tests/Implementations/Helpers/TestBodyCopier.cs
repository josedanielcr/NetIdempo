using System.Text;
using Microsoft.AspNetCore.Http;
using NetIdempo.Implementations.Helpers;

namespace NetIdempo.Tests.Implementations.Helpers;

public class TestBodyCopier
{
    [Fact]
    public void CopyBody_ShouldNotAddEmptyBodyToRequest()
    {
        // Arrange
        HttpContext context = new DefaultHttpContext();
        HttpRequestMessage request = new HttpRequestMessage();
        context.Request.Body = new MemoryStream();
        context.Request.Body.Write(new byte[] { }, 0, 0);
        context.Request.Body = null;

        // Act
        BodyCopier.CopyRequestBody(request, context);
        
        // Assert
        Assert.Equal(context.Request.Body, request.Content?.ReadAsStreamAsync().Result);
    }
    
    [Fact]
    public async Task CopyBody_ShouldAddBodyToRequest()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var request = new HttpRequestMessage();

        byte[] content = Encoding.UTF8.GetBytes("Test content");
        context.Request.Body = new MemoryStream(content);
        context.Request.Body.Position = 0;

        // Act
        BodyCopier.CopyRequestBody(request, context);

        // Assert
        Assert.NotNull(request.Content);

        var bodyString = await request.Content.ReadAsStringAsync();
        Assert.Equal("Test content", bodyString);

        Assert.Equal(content.Length, request.Content.Headers.ContentLength);
    }
}