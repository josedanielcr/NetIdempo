using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NetIdempo.Abstractions.Helpers.Config;
using NetIdempo.Abstractions.Helpers.HttpUtils;
using NetIdempo.Common;
using NetIdempo.Implementations.Helpers.Config;

namespace NetIdempo.Implementations.Helpers.HttpUtils;

public class HttpRequestBuilder(IContextReader contextReader,
    IHttpRequestCopier httpRequestCopier,
    IOptionsReader optionsReader) : IHttpRequestBuilder
{
    
    public string GetDestinationServiceUrlFromOptions(string serviceKey)
    {
        var baseUrl = optionsReader.GetDestinationServiceBaseUrlFromOptions(serviceKey);
        if (string.IsNullOrEmpty(baseUrl))
        {
            throw new ArgumentException($"Service with key '{serviceKey}' not found or has no base URL configured.");
        }
        return baseUrl;
    }
    
    public async Task<HttpRequestMessage> CreateFromContextAsync(HttpContext context, string baseUrl, string serviceKey)
    {
        var destinationUrl = GetDestinationServiceUrl(context, serviceKey);
        var request = BuildFinalRequest(context, baseUrl, destinationUrl);
        await httpRequestCopier.CopyFromHttpContextAsync(context, request);
        return request;
    }
    
    private string GetDestinationServiceUrl(HttpContext context, string serviceKey)
    {
        var servicePrefix = optionsReader.GetPathPrefixByServiceKey(serviceKey);
        var remainingPath = contextReader.GetDestinationFinalUrlFromContext(context, servicePrefix);
        return remainingPath;
    }
    
    private HttpRequestMessage BuildFinalRequest(HttpContext context, string baseUrl, string remainingPath)
    {
        var method = new HttpMethod(context.Request.Method);
        var request = new HttpRequestMessage(method, new Uri(new Uri(baseUrl), remainingPath + context.Request.QueryString));
        return request;
    }
}