using Microsoft.AspNetCore.Http;

namespace NetIdempo.Abstractions.Helpers;

public interface IRequestBuilder
{
    Task<HttpRequestMessage> BuildRequest(HttpContext context, string serviceKey);
}