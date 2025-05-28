using Microsoft.AspNetCore.Http;
using NetIdempo.Implementations.Helpers;

namespace NetIdempo.Tests.Implementations.Helpers;

public class TestHeaderCopier
{
    [Fact]
    public void CopyContextHeaders_ShouldTransferNonRestrictedHeadersToHttpRequestMessage()
    {
        //Arrange
        var context = new DefaultHttpContext();
        var request = new HttpRequestMessage();
        context.Request.Headers.Add("Test-Header", "TestValue");
        context.Request.Headers.Add("Another-Header", "AnotherValue");
        
        //Act
        HeaderCopier.CopyContextHeadersToRequest(context, request);
        
        //Assert
        Assert.True(request.Headers.Contains("Test-Header"));
        Assert.Equal("TestValue", request.Headers.GetValues("Test-Header").First());
        Assert.True(request.Headers.Contains("Another-Header"));
        Assert.Equal("AnotherValue", request.Headers.GetValues("Another-Header").First());
    }
    
    [Fact]
    public void CopyContextHeaders_ShouldNotTransferRestrictedHeadersToHttpRequestMessage()
    {
        //Arrange
        var context = new DefaultHttpContext();
        var request = new HttpRequestMessage();
        context.Request.Headers.Add("Host", "example.com");
        
        //Act
        HeaderCopier.CopyContextHeadersToRequest(context, request);
        
        //Assert
        Assert.NotEqual("example.com", request.Headers.Host);
    }
    
    [Fact]
    public void CopyResponseHeaders_ShouldTransferNonRestrictedHeadersToHttpContext()
    {
        //Arrange
        var context = new DefaultHttpContext();
        var response = new HttpResponseMessage();
        response.Headers.Add("Test-Header", "TestValue");
        response.Headers.Add("Another-Header", "AnotherValue");
        
        //Act
        HeaderCopier.CopyResponseHeadersToContext(response, context);
        
        //Assert
        Assert.True(context.Response.Headers.ContainsKey("Test-Header"));
        Assert.Equal("TestValue", context.Response.Headers["Test-Header"]);
        Assert.True(context.Response.Headers.ContainsKey("Another-Header"));
        Assert.Equal("AnotherValue", context.Response.Headers["Another-Header"]);
    }
    
    [Fact]
    public void CopyResponseHeaders_ShouldNotTransferRestrictedHeadersToHttpContext()
    {
        //Arrange
        var context = new DefaultHttpContext();
        var response = new HttpResponseMessage();
        response.Content.Headers.Add("Content-Length", "1234");
        
        //Act
        HeaderCopier.CopyResponseHeadersToContext(response, context);
        
        //Assert
        Assert.False(context.Response.Headers.ContainsKey("Content-Length"));
    }
}