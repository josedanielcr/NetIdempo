using Microsoft.AspNetCore.Http;

namespace NetIdempo.Abstractions.Helpers;

public interface IRequestBuilder
{
    HttpRequestMessage BuildRequest(HttpContext context, string serviceKey);
}