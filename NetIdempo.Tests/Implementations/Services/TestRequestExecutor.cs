using System.Net;
using Microsoft.AspNetCore.Http;

namespace NetIdempo.Tests.Implementations.Services;

public class TestRequestExecutor
{
    [Fact]
    public async Task ExecuteAsync_ShouldThrowArgumentNullException_WhenRequestIsNull()
    {
        // Arrange
        var executor = new NetIdempo.Implementations.Services.RequestExecutor(new HttpClient());
        HttpRequestMessage? request = null;
        var context = new DefaultHttpContext();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => executor.ExecuteAsync(request, context));
    }
    
    [Fact]
    public async Task ExecuteAsync_ShouldThrowArgumentNullException_WhenContextIsNull()
    {
        // Arrange
        var executor = new NetIdempo.Implementations.Services.RequestExecutor(new HttpClient());
        var request = new HttpRequestMessage(HttpMethod.Get, "http://example.com");
        HttpContext? context = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => executor.ExecuteAsync(request, context));
    }
    
    [Fact]
    public async Task ExecuteAsync_ShouldReturnResponse_WhenRequestIsValid()
    {
        // Arrange
        var executor = new NetIdempo.Implementations.Services.RequestExecutor(new HttpClient());
        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5262/weatherforecast");
        var context = new DefaultHttpContext();

        // Act
        var response = await executor.ExecuteAsync(request, context);

        // Assert
        Assert.NotNull(response);
    }
}