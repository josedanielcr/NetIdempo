using Microsoft.AspNetCore.Http;

namespace NetIdempo.Abstractions.Helpers.Body;

public interface IResponseBodyCopier
{
    Task CopyToHttpContextAsync(HttpResponseMessage response, HttpContext context);
}