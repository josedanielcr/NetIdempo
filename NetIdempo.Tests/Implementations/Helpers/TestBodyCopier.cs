using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Http;
using NetIdempo.Implementations.Helpers;

namespace NetIdempo.Tests.Implementations.Helpers;

public class TestBodyCopier
{
    [Fact]
    public void CopyEmptyRequestBody_ShouldCreateEmptyHttpRequestMessageContent()
    {
        // Arrange
        HttpContext context = new DefaultHttpContext();
        HttpRequestMessage request = new HttpRequestMessage();
        context.Request.Body = new MemoryStream();
        context.Request.Body.Write(new byte[] { }, 0, 0);
        context.Request.Body = null;

        // Act
        BodyCopier.CopyContextBodyToRequest(request, context);
        
        // Assert
        Assert.Equal(context.Request.Body, request.Content?.ReadAsStreamAsync().Result);
    }
    
    [Fact]
    public async Task CopyContextBodyToRequest_ShouldTransferBodyToHttpRequestMessage()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var request = new HttpRequestMessage();

        byte[] content = Encoding.UTF8.GetBytes("Test content");
        context.Request.Body = new MemoryStream(content);
        context.Request.Body.Position = 0;

        // Act
        BodyCopier.CopyContextBodyToRequest(request, context);

        // Assert
        Assert.NotNull(request.Content);

        var bodyString = await request.Content.ReadAsStringAsync();
        Assert.Equal("Test content", bodyString);

        Assert.Equal(content.Length, request.Content.Headers.ContentLength);
    }
    
    [Fact]
    public async Task CopyResponseBodyToContext_ShouldTransferBodyToHttpContextResponse()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream(); // Use in-memory body stream

        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("Response content")
        };

        // Act
        await BodyCopier.CopyResponseBodyToContext(response, context);

        // Assert
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var bodyContent = await new StreamReader(context.Response.Body).ReadToEndAsync();
        Assert.Equal("Response content", bodyContent);
    }
}