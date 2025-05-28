using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NetIdempo.Common;
using NetIdempo.Implementations.Core;
using NetIdempo.Implementations.Helpers;
using NetIdempo.Implementations.Services;
using NetIdempo.Implementations.Store;

namespace NetIdempo.Tests.Implementations.Core;

public class TestRequestReceiver
{
    [Fact]
    public async Task HandleRequest_ShouldReturnSameContext()
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
        
        var reader = new ContextReader(options);
        var idempotencyStore = new IdempotencyStore();
        var requestBuilder = new RequestBuilder(options);
        var requestExecutor = new RequestExecutor(new HttpClient());
        var forwarder = new RequestForwarder(options, reader,requestBuilder, requestExecutor );
        var contextReader = new ContextReader(options);
        var requestProcessor = new RequestProcessor(options, contextReader, forwarder, idempotencyStore );
        var context = new DefaultHttpContext();
        var handler = new RequestReceiver(requestProcessor);
        
        // Simulate a request
        context.Request.Path = new PathString("/testapi/resource");
        context.Request.Method = "GET";

        // Act
        var result = await handler.ReceiveRequestAsync(context);

        // Assert
        Assert.Equal(context, result);
    }
}