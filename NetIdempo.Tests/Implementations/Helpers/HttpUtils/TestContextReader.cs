using Microsoft.AspNetCore.Http;
using Moq;
using NetIdempo.Abstractions.Helpers.Config;
using NetIdempo.Implementations.Helpers.HttpUtils;

namespace NetIdempo.Tests.Implementations.Helpers.HttpUtils;

public class TestContextReader
{
    [Fact]
    public void IsIdempotencyKeyPresent_ShouldReturnTrue_WhenHeaderExists()
    {
        // Arrange
        var optionsReader = new Mock<IOptionsReader>();
        optionsReader.Setup(o => o.GetIdempotencyKeyHeader()).Returns("Idempotency-Key");
        
        var contextReader = new ContextReader(optionsReader.Object);
        var context = new DefaultHttpContext();
        context.Request.Headers["Idempotency-Key"] = "test-key";

        // Act
        var result = contextReader.IsIdempotencyKeyPresent(context);

        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void IsIdempotencyKeyPresent_ShouldReturnFalse_WhenHeaderDoesNotExist()
    {
        // Arrange
        var optionsReader = new Mock<IOptionsReader>();
        optionsReader.Setup(o => o.GetIdempotencyKeyHeader()).Returns("Idempotency-Key");
        
        var contextReader = new ContextReader(optionsReader.Object);
        var context = new DefaultHttpContext();

        // Act
        var result = contextReader.IsIdempotencyKeyPresent(context);

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void GetDestinationServiceFromHttpContext_ShouldReturnServiceKey_WhenPathIsValid()
    {
        // Arrange
        var optionsReader = new Mock<IOptionsReader>();
        optionsReader.Setup(o => o.GetServiceKeyByIncomingRequestPath(It.IsAny<string>())).Returns("TestService");
        
        var contextReader = new ContextReader(optionsReader.Object);
        var context = new DefaultHttpContext();
        context.Request.Path = "/api/test";

        // Act
        var result = contextReader.GetDestinationServiceFromHttpContext(context);

        // Assert
        Assert.Equal("TestService", result);
    }
    
    [Fact]
    public void GetDestinationServiceFromHttpContext_ShouldThrowException_WhenPathIsInvalid()
    {
        // Arrange
        var optionsReader = new Mock<IOptionsReader>();
        optionsReader.Setup(o => o.GetServiceKeyByIncomingRequestPath(It.IsAny<string>())).Returns(string.Empty);
        
        var contextReader = new ContextReader(optionsReader.Object);
        var context = new DefaultHttpContext();
        context.Request.Path = "/api/invalid";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => contextReader.GetDestinationServiceFromHttpContext(context));
    }
    
    [Fact]
    public void GetKeyFromHttpRequest_ShouldReturnKey_WhenHeaderExists()
    {
        // Arrange
        var optionsReader = new Mock<IOptionsReader>();
        optionsReader.Setup(o => o.GetIdempotencyKeyHeader()).Returns("Idempotency-Key");
        
        var contextReader = new ContextReader(optionsReader.Object);
        var context = new DefaultHttpContext();
        context.Request.Headers["Idempotency-Key"] = "test-key";

        // Act
        var result = contextReader.GetKeyFromHttpRequest(context);

        // Assert
        Assert.Equal("test-key", result);
    }
    
    [Fact]
    public void GetKeyFromHttpRequest_ShouldThrowException_WhenHeaderDoesNotExist()
    {
        // Arrange
        var optionsReader = new Mock<IOptionsReader>();
        optionsReader.Setup(o => o.GetIdempotencyKeyHeader()).Returns("Idempotency-Key");
        
        var contextReader = new ContextReader(optionsReader.Object);
        var context = new DefaultHttpContext();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => contextReader.GetKeyFromHttpRequest(context));
    }
}