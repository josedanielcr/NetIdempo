using Microsoft.AspNetCore.Http;
using NetIdempo.Abstractions.Services;

namespace NetIdempo.Implementations.Services;

public class RequestExecutor(HttpClient httpClient) : IRequestExecutor
{
    public async Task ExecuteAsync(HttpRequestMessage request, HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(context);

        var result = await httpClient.SendAsync(request, context.RequestAborted);
    }
}