using Microsoft.AspNetCore.Http;

namespace NetIdempo.Abstractions.Helpers.Headers;

public interface IRequestHeaderCopier
{
    void CopyFromHttpContext(HttpRequestMessage request, HttpContext context);
}