using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NetIdempo.Abstractions.Core;
using NetIdempo.Abstractions.Helpers;
using NetIdempo.Abstractions.Services;
using NetIdempo.Common;

namespace NetIdempo.Implementations.Services;

public class RequestForwarder(IOptions<NetIdempoOptions> options, IContextReader reader,
    IRequestBuilder requestBuilder, IRequestExecutor requestExecutor) : IRequestForwarder
{
    public async Task<HttpContext> ForwardRequestAsync(HttpContext context)
    {
        var serviceKey = reader.GetServiceByPath(context);
        HttpRequestMessage request = requestBuilder.BuildRequest(context, serviceKey); 
        await requestExecutor.ExecuteAsync(request, context);
        return context;
    }
}