using Microsoft.AspNetCore.Http;

namespace NetIdempo.Abstractions.Helpers.HttpUtils;

public interface IHttpRequestCopier
{
    Task CopyFromHttpContextAsync(HttpContext context, HttpRequestMessage request);
}