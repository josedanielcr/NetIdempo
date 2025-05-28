using Microsoft.Extensions.Options;
using NetIdempo.Common;
using NetIdempo.Implementations.Services;

namespace NetIdempo.Tests.Implementations.Helpers;

public class TestRequestBuilder
{
    [Fact]
    public void GetBaseUrlFromServiceKey_ShouldReturnBaseUrl_WhenServiceKeyExists()
    {
        // Arrange
        var options = Options.Create(new NetIdempoOptions
        {
            IdempotencyKeyHeader = "Idempotency-Key",
            IdempotencyKeyLifetime = 60,
            Services = new Dictionary<string, ServiceConfig>
            {
                { "TestService1", new ServiceConfig { BaseUrl = "http://localhost:5262", Key = "testservice1" } }
            }
        });
        
        var requestBuilder = new RequestBuilder(options);

        // Act
        var baseUrl = requestBuilder.GetBaseUrlFromServiceKey("TestService1");

        // Assert
        Assert.Equal("http://localhost:5262", baseUrl);
    }
}