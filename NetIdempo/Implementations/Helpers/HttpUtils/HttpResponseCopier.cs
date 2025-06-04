using Microsoft.AspNetCore.Http;
using NetIdempo.Abstractions.Helpers.Body;
using NetIdempo.Abstractions.Helpers.Headers;
using NetIdempo.Abstractions.Helpers.HttpUtils;

namespace NetIdempo.Implementations.Helpers.HttpUtils;

public class HttpResponseCopier(IResponseHeaderCopier responseHeaderCopier,
    IResponseBodyCopier responseBodyCopier) : IHttpResponseCopier
{
    public async Task CopyToHttpContext(HttpContext context, HttpResponseMessage result)
    {
        responseHeaderCopier.CopyToHttpContext(result,context);
        await responseBodyCopier.CopyToHttpContextAsync(result,context);
    }
}