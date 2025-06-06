using Microsoft.AspNetCore.Http;

namespace NetIdempo.Abstractions.Helpers.Headers;

public interface IResponseHeaderCopier
{
    void CopyToHttpContext(HttpResponseMessage response, HttpContext context);
}