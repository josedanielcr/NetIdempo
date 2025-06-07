using Microsoft.AspNetCore.Http;
using Moq;
using NetIdempo.Abstractions.Helpers;
using NetIdempo.Abstractions.Helpers.HttpUtils;
using NetIdempo.Abstractions.Services;
using NetIdempo.Implementations.Services;

namespace NetIdempo.Tests.Implementations.Services;

public class TestRequestForwarder
{
    [Fact]
    public async Task ForwardRequestAsync_ShouldForwardRequest()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Method = "GET";
        context.Request.Path = "/test";

        var reader = new Mock<IContextReader>();
        var requestBuilder = new Mock<IRequestBuilder>();
        var requestExecutor = new Mock<IRequestExecutor>();

        reader.Setup(r => r.GetDestinationServiceFromHttpContext(context)).Returns("TestService");
        requestBuilder.Setup(rb => rb.BuildRequest(context, "TestService"))
            .ReturnsAsync(new HttpRequestMessage(HttpMethod.Get, "http://example.com/test"));

        var forwarder = new RequestForwarder(reader.Object, requestBuilder.Object, requestExecutor.Object);

        // Act
        await forwarder.ForwardRequestAsync(context);

        // Assert
        requestExecutor.Verify(re => re.ExecuteAsync(It.IsAny<HttpRequestMessage>(), context), Times.Once);
    }
}