using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NetIdempo.Common;
using NetIdempo.Implementations.Core;

namespace NetIdempo.Tests.Implementations.Core;

public class TestRequestHandler
{
    [Fact]
    public void HandleRequest_ShouldReturnSameContext()
    {
        // Arrange
        var options = Options.Create(new NetIdempoOptions
        {
            IdempotencyKeyHeader = "Idempotency-Key",
            IdempotencyKeyLifetime = 60,
            Services = new Dictionary<string, ServiceConfig>
            {
                { "TestApi", new ServiceConfig { BaseUrl = "https://testapi" } }
            }
        });
        
        var context = new DefaultHttpContext();
        var handler = new RequestHandler(options);

        // Act
        var result = handler.HandleRequest(context);

        // Assert
        Assert.Equal(context, result);
    }
}