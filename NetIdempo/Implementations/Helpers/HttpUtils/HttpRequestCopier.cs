using Microsoft.AspNetCore.Http;
using NetIdempo.Abstractions.Helpers.Body;
using NetIdempo.Abstractions.Helpers.Headers;
using NetIdempo.Abstractions.Helpers.HttpUtils;

namespace NetIdempo.Implementations.Helpers.HttpUtils;

public class HttpRequestCopier(IRequestHeaderCopier requestHeaderCopier,
    IRequestBodyCopier requestBodyCopier) : IHttpRequestCopier
{
    public async Task CopyFromHttpContextAsync(HttpContext context, HttpRequestMessage request)
    {
        requestHeaderCopier.CopyFromHttpContext(request, context);
        await requestBodyCopier.CopyFromHttpContextAsync(request, context);
    }
}