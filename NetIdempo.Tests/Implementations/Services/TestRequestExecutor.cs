using Microsoft.AspNetCore.Http;
using Moq;
using NetIdempo.Abstractions.Helpers.Body;
using NetIdempo.Abstractions.Helpers.Headers;
using NetIdempo.Implementations.Helpers.HttpUtils;

namespace NetIdempo.Tests.Implementations.Services;

public class TestRequestExecutor
{
    [Fact]
    public async Task ExecuteAsync_ShouldThrowArgumentNullException_WhenRequestIsNull()
    {
        // Arrange
         var responseHeaderCopier = new Mock<IResponseHeaderCopier>();
        var responseBodyCopier = new Mock<IResponseBodyCopier>();
        var executor = new NetIdempo.Implementations.Services.RequestExecutor(
            new HttpClient(),
            new HttpResponseCopier(responseHeaderCopier.Object,responseBodyCopier.Object));

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => executor.ExecuteAsync(null, new DefaultHttpContext()));
    }
    
    [Fact]
    public async Task ExecuteAsync_ShouldThrowArgumentNullException_WhenContextIsNull()
    {
        // Arrange
        var responseHeaderCopier = new Mock<IResponseHeaderCopier>();
        var responseBodyCopier = new Mock<IResponseBodyCopier>();
        var executor = new NetIdempo.Implementations.Services.RequestExecutor(
            new HttpClient(),
            new HttpResponseCopier(responseHeaderCopier.Object,responseBodyCopier.Object));

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => executor.ExecuteAsync(new HttpRequestMessage(), null));
    }
    
    [Fact]
    public async Task ExecuteAsync_ShouldExecuteHttpRequest_WhenRequestAndContextAreNotNull()
    {
        // Arrange
        var responseHeaderCopier = new Mock<IResponseHeaderCopier>();
        var responseBodyCopier = new Mock<IResponseBodyCopier>();
        var executor = new NetIdempo.Implementations.Services.RequestExecutor(
            new HttpClient(),
            new HttpResponseCopier(responseHeaderCopier.Object,responseBodyCopier.Object));
        
        var request = new HttpRequestMessage(HttpMethod.Get, "http://example.com");
        var context = new DefaultHttpContext();

        // Act
        await executor.ExecuteAsync(request, context);

        // Assert
        responseHeaderCopier.Verify(c => c.CopyToHttpContext(It.IsAny<HttpResponseMessage>(), context), Times.Once);
        responseBodyCopier.Verify(c => c.CopyToHttpContextAsync(It.IsAny<HttpResponseMessage>(), context), Times.Once);
    }
}