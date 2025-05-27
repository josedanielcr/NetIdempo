using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NetIdempo.Abstractions.Helpers;
using NetIdempo.Common;
using NetIdempo.Implementations.Helpers;

namespace NetIdempo.Implementations.Services;

public class RequestBuilder(IOptions<NetIdempoOptions> options) : IRequestBuilder
{
    public HttpRequestMessage BuildRequest(HttpContext context, string serviceKey)
    {
        var baseUrl = GetBaseUrlFromServiceKey(serviceKey);
        if (string.IsNullOrEmpty(baseUrl))
        {
            throw new ArgumentException($"Service with key '{serviceKey}' not found or has no base URL configured.");
        }
        return BuildHttpRequestMessage(context, baseUrl);
    }

    public string GetBaseUrlFromServiceKey(string serviceKey)
    {
        return options.Value.Services.FirstOrDefault(service => service.Key == serviceKey).Value.BaseUrl;
    }

    private HttpRequestMessage BuildHttpRequestMessage(HttpContext context, string baseUrl)
    {
        var method = new HttpMethod(context.Request.Method);
        var request = new HttpRequestMessage(method, new Uri(new Uri(baseUrl), context.Request.Path + context.Request.QueryString));
        HeaderCopier.CopyHeaders(request, context);
        BodyCopier.CopyRequestBody(request, context);
        return request;
    }
}