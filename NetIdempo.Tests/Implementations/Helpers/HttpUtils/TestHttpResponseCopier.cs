using Microsoft.AspNetCore.Http;
using Moq;
using NetIdempo.Abstractions.Helpers.Body;
using NetIdempo.Abstractions.Helpers.Headers;
using NetIdempo.Implementations.Helpers.HttpUtils;

namespace NetIdempo.Tests.Implementations.Helpers.HttpUtils;

public class TestHttpResponseCopier
{
    [Fact]
    public async Task CopyToHttpContext_ShouldCallHeaderAndBodyCopiers()
    {
        // Arrange
        var responseHeaderCopier = new Mock<IResponseHeaderCopier>();
        var responseBodyCopier = new Mock<IResponseBodyCopier>();
        var httpResponseCopier = new HttpResponseCopier(responseHeaderCopier.Object, responseBodyCopier.Object);
        var context = new DefaultHttpContext();
        var result = new HttpResponseMessage();

        // Act
        await httpResponseCopier.CopyToHttpContext(context, result);

        // Assert
        responseHeaderCopier.Verify(c => c.CopyToHttpContext(result, context), Times.Once);
        responseBodyCopier.Verify(c => c.CopyToHttpContextAsync(result, context), Times.Once);
    }
}