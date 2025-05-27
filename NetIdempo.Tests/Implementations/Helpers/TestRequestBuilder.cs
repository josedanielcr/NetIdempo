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
                { "TestApi", new ServiceConfig { BaseUrl = "https://testapi", Key = "testapi" } }
            }
        });
        
        var requestBuilder = new RequestBuilder(options);

        // Act
        var baseUrl = requestBuilder.GetBaseUrlFromServiceKey("TestApi");

        // Assert
        Assert.Equal("https://testapi", baseUrl);
    }
}