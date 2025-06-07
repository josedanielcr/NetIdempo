using Microsoft.AspNetCore.Http;
using Moq;
using NetIdempo.Abstractions.Helpers.HttpUtils;
using NetIdempo.Implementations.Services;

namespace NetIdempo.Tests.Implementations.Services;

public class TestRequestBuilder
{
    [Fact]
    public async Task BuildRequest_ShouldReturnHttpRequestMessage()
    {
        // Arrange
        var httpRequestBuilder = new Mock<IHttpRequestBuilder>();
        var requestBuilder = new RequestBuilder(httpRequestBuilder.Object);
        var context = new DefaultHttpContext();
        var serviceKey = "testService";

        httpRequestBuilder
            .Setup(x => x.GetDestinationServiceUrlFromOptions(serviceKey))
            .Returns("http://example.com");

        httpRequestBuilder
            .Setup(x => x.CreateFromContextAsync(context, "http://example.com", serviceKey))
            .ReturnsAsync(new HttpRequestMessage(HttpMethod.Get, "http://example.com"));

        // Act
        var result = await requestBuilder.BuildRequest(context, serviceKey);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("http://example.com/", result.RequestUri.ToString());
    }
}