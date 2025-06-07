using Microsoft.AspNetCore.Http;
using Moq;
using NetIdempo.Abstractions.Helpers.Cache;
using NetIdempo.Abstractions.Helpers.Config;
using NetIdempo.Abstractions.Helpers.HttpUtils;
using NetIdempo.Abstractions.Services;
using NetIdempo.Abstractions.Store;
using NetIdempo.Common.Store;
using NetIdempo.Implementations.Core;

namespace NetIdempo.Tests.Implementations.Core;

public class TestRequestProcessor
{
    [Fact]
    public async Task ProcessRequestAsync_ShouldForwardNonIdempotentRequest()
    {
        // Arrange
        var contextReader = new Mock<IContextReader>();
        var forwarder = new Mock<IRequestForwarder>();
        var idempotencyStore = new Mock<IIdempotencyStore>();
        var cacheResponseApplier = new Mock<ICacheResponseApplier>();
        var cacheEntryFactory = new Mock<ICacheEntryFactory>();
        var optionsReader = new Mock<IOptionsReader>();

        var processor = new RequestProcessor(contextReader.Object, forwarder.Object, idempotencyStore.Object,
            cacheResponseApplier.Object, cacheEntryFactory.Object, optionsReader.Object);

        var context = new DefaultHttpContext();
        context.Request.Headers["Idempotency-Key"] = "test-key";

        contextReader.Setup(x => x.IsIdempotencyKeyPresent(context)).Returns(false);

        // Act
        await processor.ProcessRequestAsync(context);

        // Assert
        forwarder.Verify(x => x.ForwardRequestAsync(context), Times.Once);
    }
    
    [Fact]
    public async Task ProcessRequestAsync_ShouldProcessNewIdempotentRequest()
    {
        // Arrange
        var contextReader = new Mock<IContextReader>();
        var forwarder = new Mock<IRequestForwarder>();
        var idempotencyStore = new Mock<IIdempotencyStore>();
        var cacheResponseApplier = new Mock<ICacheResponseApplier>();
        var cacheEntryFactory = new Mock<ICacheEntryFactory>();
        var optionsReader = new Mock<IOptionsReader>();

        var processor = new RequestProcessor(contextReader.Object, forwarder.Object, idempotencyStore.Object,
            cacheResponseApplier.Object, cacheEntryFactory.Object, optionsReader.Object);

        var context = new DefaultHttpContext();
        context.Request.Headers["Idempotency-Key"] = "test-key";

        contextReader.Setup(x => x.IsIdempotencyKeyPresent(context)).Returns(true);
        contextReader.Setup(x => x.GetKeyFromHttpRequest(context)).Returns("test-key");
        idempotencyStore.Setup(x => x.GetAsync("test-key")).ReturnsAsync((IdempotencyCacheEntry?)null);
        optionsReader.Setup(x => x.GetIdempotencyKeyHeader()).Returns("Idempotency-Key");
        cacheEntryFactory.Setup(x => x.CreateEmpty("test-key")).Returns(new IdempotencyCacheEntry
        {
            Key = "test-key",
            IsCompleted = false
        });

        // Act
        await processor.ProcessRequestAsync(context);

        // Assert
        forwarder.Verify(x => x.ForwardRequestAsync(context), Times.Once);
    }
}