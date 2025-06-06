using Microsoft.AspNetCore.Http;

namespace NetIdempo.Abstractions.Helpers.Body;

public interface IRequestBodyCopier
{
    Task<HttpRequestMessage> CopyFromHttpContextAsync(HttpRequestMessage request, HttpContext context);
}