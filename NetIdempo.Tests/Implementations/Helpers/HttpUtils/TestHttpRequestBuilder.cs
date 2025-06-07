using Microsoft.AspNetCore.Http;
using Moq;
using NetIdempo.Abstractions.Helpers.Config;
using NetIdempo.Abstractions.Helpers.HttpUtils;
using NetIdempo.Implementations.Helpers.HttpUtils;

namespace NetIdempo.Tests.Implementations.Helpers.HttpUtils;

public class TestHttpRequestBuilder
{
    [Fact]
    public void GetDestinationServiceUrlFromOptions_ShouldReturnBaseUrl_WhenServiceKeyExists()
    {
        // Arrange
        var optionsReader = new Mock<IOptionsReader>();
        optionsReader.Setup(o => o.GetDestinationServiceBaseUrlFromOptions("TestService"))
            .Returns("http://testservice.com");

        var contextReader = new Mock<IContextReader>();
        var httpRequestCopier = new Mock<IHttpRequestCopier>();

        var requestBuilder = new HttpRequestBuilder(contextReader.Object, httpRequestCopier.Object, optionsReader.Object);

        // Act
        var result = requestBuilder.GetDestinationServiceUrlFromOptions("TestService");

        // Assert
        Assert.Equal("http://testservice.com", result);
    }
    
    [Fact]
    public void GetDestinationServiceUrlFromOptions_ShouldThrowArgumentException_WhenServiceKeyDoesNotExist()
    {
        // Arrange
        var optionsReader = new Mock<IOptionsReader>();
        optionsReader.Setup(o => o.GetDestinationServiceBaseUrlFromOptions("NonExistentService"))
            .Returns((string)null);

        var contextReader = new Mock<IContextReader>();
        var httpRequestCopier = new Mock<IHttpRequestCopier>();

        var requestBuilder = new HttpRequestBuilder(contextReader.Object, httpRequestCopier.Object, optionsReader.Object);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => requestBuilder.GetDestinationServiceUrlFromOptions("NonExistentService"));
    }
    
    [Fact]
    public Task CreateFromContextAsync_ShouldCreateHttpRequestMessage_WhenContextIsValid()
    {
        // Arrange
        var optionsReader = new Mock<IOptionsReader>();
        optionsReader.Setup(o => o.GetDestinationServiceBaseUrlFromOptions("TestService"))
            .Returns("http://testservice.com");

        var contextReader = new Mock<IContextReader>();
        contextReader.Setup(cr => cr.GetDestinationFinalUrlFromContext(It.IsAny<HttpContext>(), It.IsAny<string>()))
            .Returns("/api/resource");

        var httpRequestCopier = new Mock<IHttpRequestCopier>();
        var requestBuilder = new HttpRequestBuilder(contextReader.Object, httpRequestCopier.Object, optionsReader.Object);

        var context = new DefaultHttpContext();
        context.Request.Method = "GET";
        context.Request.QueryString = new QueryString("?query=param");
        // Act
        return requestBuilder.CreateFromContextAsync(context, "http://testservice.com", "TestService")
            .ContinueWith(task =>
            {
                // Assert
                var request = task.Result;
                Assert.Equal(HttpMethod.Get, request.Method);
                Assert.Equal("http://testservice.com/api/resource?query=param", request.RequestUri.ToString());
            });
    }
}