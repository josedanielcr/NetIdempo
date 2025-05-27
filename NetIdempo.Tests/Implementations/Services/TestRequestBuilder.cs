using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NetIdempo.Common;

namespace NetIdempo.Tests.Implementations.Services;

public class TestRequestBuilder
{
    [Fact]
    public void BuildRequest_ShouldReturnGetHttpRequestMessage()
    {
        // Arrange
        var options = Options.Create(new NetIdempoOptions
        {
            IdempotencyKeyHeader = "Idempotency-Key",
            IdempotencyKeyLifetime = 60,
            Services = new Dictionary<string, ServiceConfig>
            {
                { "TestApi", new ServiceConfig { BaseUrl = "https://testapi", Key = "testapi" } }
            }
        });
        var context = new DefaultHttpContext();
        context.Request.Method = "GET";
        context.Request.Path = "/test";
        context.Request.QueryString = new QueryString("?param=value");
        var serviceKey = "TestApi";

        var requestBuilder = new NetIdempo.Implementations.Services.RequestBuilder(options);

        // Act
        var requestMessage = requestBuilder.BuildRequest(context, serviceKey);

        // Assert
        Assert.NotNull(requestMessage);
        Assert.Equal("GET", requestMessage.Method.Method);
        Assert.Equal("https://testapi/test?param=value", requestMessage.RequestUri?.ToString());
    }
    
    [Fact]
    public async Task BuildRequest_ShouldReturnPostHttpRequestMessage()
    {
        // Arrange
        var options = Options.Create(new NetIdempoOptions
        {
            IdempotencyKeyHeader = "Idempotency-Key",
            IdempotencyKeyLifetime = 60,
            Services = new Dictionary<string, ServiceConfig>
            {
                { "TestApi", new ServiceConfig { BaseUrl = "https://testapi", Key = "testapi" } }
            }
        });
        var context = new DefaultHttpContext();
        context.Request.Method = "POST";
        context.Request.Path = "/test";
        context.Request.QueryString = new QueryString("?param=value");
        context.Request.Body = new MemoryStream(System.Text.Encoding.UTF8.GetBytes("Test body"));
        var serviceKey = "TestApi";

        var requestBuilder = new NetIdempo.Implementations.Services.RequestBuilder(options);

        // Act
        var requestMessage = requestBuilder.BuildRequest(context, serviceKey);

        // Assert
        var bodyString = await requestMessage.Content?.ReadAsStringAsync()!;
        
        Assert.NotNull(requestMessage);
        Assert.Equal("POST", requestMessage.Method.Method);
        Assert.Equal("https://testapi/test?param=value", requestMessage.RequestUri?.ToString());
        Assert.Equal("Test body",bodyString);
    }
}