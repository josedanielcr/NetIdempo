using Microsoft.AspNetCore.Http;

namespace NetIdempo.Abstractions.Helpers.HttpUtils;

public interface IHttpResponseCopier
{
    Task CopyToHttpContext(HttpContext context, HttpResponseMessage result);
}