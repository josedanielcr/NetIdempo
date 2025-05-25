using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NetIdempo.Common;
using NetIdempo.Implementations.Core;
using NetIdempo.Implementations.Helpers;

namespace NetIdempo.Tests.Implementations.Core;

public class TestRequestReceiver
{
    [Fact]
    public async Task HandleRequest_ShouldReturnSameContext()
    {
        try
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
            var reader = new ContextReader(options);
            var idempotencyStore = new IdempotencyStore();
            var forwarder = new RequestForwarder(options, reader);
            var contextReader = new ContextReader(options);
            var requestProcessor = new RequestProcessor(options, contextReader, forwarder, idempotencyStore );
            var context = new DefaultHttpContext();
            var handler = new RequestReceiver(requestProcessor);

            // Act
            var result = await handler.ReceiveRequestAsync(context);

            // Assert
            Assert.Equal(context, result);
        }
        catch (Exception ex)
        {
            Assert.Fail($"Exception thrown: {ex.Message}");
        }
    }
}