using Microsoft.AspNetCore.Http;

namespace NetIdempo.Abstractions.Helpers.HttpUtils;

public interface IHttpRequestBuilder
{
    Task<HttpRequestMessage> CreateFromContextAsync(HttpContext context, string baseUrl, string serviceKey);
    string GetDestinationServiceUrlFromOptions(string serviceKey);
}