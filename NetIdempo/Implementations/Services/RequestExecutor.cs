using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NetIdempo.Abstractions.Helpers.HttpUtils;
using NetIdempo.Abstractions.Services;
using NetIdempo.Common;
using NetIdempo.Implementations.Helpers;

namespace NetIdempo.Implementations.Services;

public class RequestExecutor(HttpClient httpClient, IHttpResponseCopier responseCopier) : IRequestExecutor
{
    public async Task ExecuteAsync(HttpRequestMessage request, HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(context);
        var result = await ExecuteHttpRequest(request, context);
        await responseCopier.CopyToHttpContext(context, result);
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