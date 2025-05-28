using Microsoft.AspNetCore.Http;

namespace NetIdempo.Abstractions.Services;

public interface IRequestExecutor
{
    Task<HttpContext> ExecuteAsync(HttpRequestMessage request, HttpContext context);
}