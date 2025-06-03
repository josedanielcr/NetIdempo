using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NetIdempo.Abstractions.Services;
using NetIdempo.Common;
using NetIdempo.Implementations.Helpers;

namespace NetIdempo.Implementations.Services;

public class RequestExecutor(HttpClient httpClient) : IRequestExecutor
{
    public async Task ExecuteAsync(HttpRequestMessage request, HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(context);
        var result = await ExecuteHttpRequest(request, context);
        await CopyHttpResponseToHttpContext(context, result);
    }

    private static async Task CopyHttpResponseToHttpContext(HttpContext context, HttpResponseMessage result)
    {
        HeaderCopier.CopyResponseHeadersToContext(result, context);
        await BodyCopier.CopyResponseBodyToContext(result, context);
    }

    private async Task<HttpResponseMessage> ExecuteHttpRequest(HttpRequestMessage request, HttpContext context)
    {
        var result = await httpClient.SendAsync(
            request,
            HttpCompletionOption.ResponseHeadersRead,
            context.RequestAborted);
        return result;
    }
}