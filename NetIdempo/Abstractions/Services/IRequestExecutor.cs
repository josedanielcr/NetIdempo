using Microsoft.AspNetCore.Http;

namespace NetIdempo.Abstractions.Services;

public interface IRequestExecutor
{
    Task ExecuteAsync(HttpRequestMessage request, HttpContext context);
}