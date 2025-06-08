using Microsoft.AspNetCore.Http;
using NetIdempo.Abstractions.Helpers;
using NetIdempo.Abstractions.Helpers.HttpUtils;

namespace NetIdempo.Implementations.Services;

public class RequestBuilder(IHttpRequestBuilder httpRequestBuilder) : IRequestBuilder
{
    public async Task<HttpRequestMessage> BuildRequest(HttpContext context, string serviceKey)
    {
        var baseUrl = httpRequestBuilder.GetDestinationServiceUrlFromOptions(serviceKey);
        return await httpRequestBuilder.CreateFromContextAsync(context, baseUrl, serviceKey);
    }
}