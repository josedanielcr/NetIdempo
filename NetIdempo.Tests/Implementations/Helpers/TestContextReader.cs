using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NetIdempo.Common;
using NetIdempo.Implementations.Helpers;

namespace NetIdempo.Tests.Implementations.Helpers;

public class TestContextReader
{
    [Fact]
    public void ReadContext_ShouldReturnIfKeyIsPresent()
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
        var contextReader = new ContextReader(options);
        var context = new DefaultHttpContext();
        context.Request.Headers["Idempotency-Key"] = "TestKey";

        // Act
        var isKeyPresent = contextReader.IsKeyPresent(context);

        // Assert
        Assert.True(isKeyPresent, "Expected Idempotency Key to be present in the context.");
    }
    
    [Fact]
    public void GetServiceByPath_ShouldReturnCorrectServiceKey()
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
        var contextReader = new ContextReader(options);
        var context = new DefaultHttpContext();
        context.Request.Path = "/testservice1";

        // Act
        var serviceKey = contextReader.GetServiceByPath(context);

        // Assert
        Assert.Equal("TestService1", serviceKey);
    }
}