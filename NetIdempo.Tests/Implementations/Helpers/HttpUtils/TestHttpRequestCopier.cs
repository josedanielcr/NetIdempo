using Microsoft.AspNetCore.Http;
using Moq;
using NetIdempo.Abstractions.Helpers.Body;
using NetIdempo.Abstractions.Helpers.Headers;
using NetIdempo.Implementations.Helpers.HttpUtils;

namespace NetIdempo.Tests.Implementations.Helpers.HttpUtils;

public class TestHttpRequestCopier
{
    [Fact]
    public Task CopyFromHttpContextAsync_ShouldCallHeaderAndBodyCopiers()
    {
        // Arrange
        var headerCopier = new Mock<IRequestHeaderCopier>();
        var bodyCopier = new Mock<IRequestBodyCopier>();
        var copier = new HttpRequestCopier(headerCopier.Object, bodyCopier.Object);
        var context = new DefaultHttpContext();
        var request = new HttpRequestMessage();

        // Act
        return copier.CopyFromHttpContextAsync(context, request)
            .ContinueWith(_ =>
            {
                // Assert
                headerCopier.Verify(c => c.CopyFromHttpContext(request, context), Times.Once);
                bodyCopier.Verify(c => c.CopyFromHttpContextAsync(request, context), Times.Once);
            });
    }
}