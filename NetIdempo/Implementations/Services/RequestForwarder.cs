using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NetIdempo.Abstractions.Core;
using NetIdempo.Abstractions.Helpers;
using NetIdempo.Abstractions.Services;
using NetIdempo.Common;
using NetIdempo.Implementations.Helpers;

namespace NetIdempo.Implementations.Services;

public class RequestForwarder(IOptions<NetIdempoOptions> options, IContextReader reader,
    IRequestBuilder requestBuilder, IRequestExecutor requestExecutor) : IRequestForwarder
{
    public async Task ForwardRequestAsync(HttpContext context)
    {
        var serviceKey = reader.GetServiceByPath(context);
        if (string.IsNullOrEmpty(serviceKey))
        {
            throw new ArgumentException("Service key not found for the given request path.");
        }
        HttpRequestMessage request = await requestBuilder.BuildRequest(context, serviceKey); 
        await requestExecutor.ExecuteAsync(request, context);
    }
}