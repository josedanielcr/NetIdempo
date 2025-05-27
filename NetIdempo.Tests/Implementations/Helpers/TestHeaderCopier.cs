using Microsoft.AspNetCore.Http;
using NetIdempo.Implementations.Helpers;

namespace NetIdempo.Tests.Implementations.Helpers;

public class TestHeaderCopier
{
    [Fact]
    public void CopyHeaders_ShouldCopyHeaders()
    {
        //Arrange
        var context = new DefaultHttpContext();
        var request = new HttpRequestMessage();
        context.Request.Headers.Add("Test-Header", "TestValue");
        context.Request.Headers.Add("Another-Header", "AnotherValue");
        
        //Act
        HeaderCopier.CopyHeaders(request, context);
        
        //Assert
        Assert.True(request.Headers.Contains("Test-Header"));
        Assert.Equal("TestValue", request.Headers.GetValues("Test-Header").First());
        Assert.True(request.Headers.Contains("Another-Header"));
        Assert.Equal("AnotherValue", request.Headers.GetValues("Another-Header").First());
    }
    
    [Fact]
    public void CopyHeaders_ShouldNotCopyRestrictedHeaders()
    {
        //Arrange
        var context = new DefaultHttpContext();
        var request = new HttpRequestMessage();
        context.Request.Headers.Add("Host", "example.com");
        
        //Act
        HeaderCopier.CopyHeaders(request, context);
        
        //Assert
        Assert.NotEqual("example.com", request.Headers.Host);
    }
}