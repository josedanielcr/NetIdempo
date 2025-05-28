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
                { "TestService1", new ServiceConfig { BaseUrl = "http://localhost:5262", Key = "testservice1" } }
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
        
        var responseStream = new MemoryStream();
        context.Response.Body = responseStream;
        
        context.Request.Path = "/testservice1/weatherforecast";
        context.Request.Method = "GET";

        // Act
        var result = await handler.ReceiveRequestAsync(context);
        
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var readerStream = new StreamReader(context.Response.Body);
        var responseBody = await readerStream.ReadToEndAsync();

        // Assert
        Assert.Equal(context, result);
        Assert.False(string.IsNullOrWhiteSpace(responseBody));
    }
}