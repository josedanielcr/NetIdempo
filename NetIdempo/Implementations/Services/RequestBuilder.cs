using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NetIdempo.Abstractions.Helpers;
using NetIdempo.Common;
using NetIdempo.Implementations.Helpers;

namespace NetIdempo.Implementations.Services;

public class RequestBuilder(IOptions<NetIdempoOptions> options) : IRequestBuilder
{
    public async Task<HttpRequestMessage> BuildRequest(HttpContext context, string serviceKey)
    {
        var baseUrl = GetBaseUrlFromServiceKey(serviceKey);
        if (string.IsNullOrEmpty(baseUrl))
        {
            throw new ArgumentException($"Service with key '{serviceKey}' not found or has no base URL configured.");
        }
        return await BuildHttpRequestMessage(context, baseUrl, serviceKey);
    }

    public string GetBaseUrlFromServiceKey(string serviceKey)
    {
        return options.Value.Services.FirstOrDefault(service => service.Key == serviceKey).Value.BaseUrl;
    }

    private async Task<HttpRequestMessage> BuildHttpRequestMessage(HttpContext context, string baseUrl, string serviceKey)
    {
        var method = new HttpMethod(context.Request.Method);
        var serviceUrlKey = GetKeyFromServiceKeyIdentifier(serviceKey);
        var index = context.Request.Path.ToString().IndexOf(serviceUrlKey, StringComparison.Ordinal);
        var remainingPath = context.Request.Path.ToString().Substring(index + serviceUrlKey.Length).Trim();
        var request = new HttpRequestMessage(method, new Uri(new Uri(baseUrl), remainingPath + context.Request.QueryString));
        HeaderCopier.CopyContextHeadersToRequest(context, request);
        await BodyCopier.CopyContextBodyToRequest(request, context);
        return request;
    }

    private string GetKeyFromServiceKeyIdentifier(string serviceKey)
    {
        return options.Value.Services.FirstOrDefault(service => service.Key == serviceKey).Value.Key;
    }
}