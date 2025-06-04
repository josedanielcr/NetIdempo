using Microsoft.AspNetCore.Http;
using NetIdempo.Abstractions.Helpers;
using NetIdempo.Abstractions.Helpers.HttpUtils;
using NetIdempo.Abstractions.Services;

namespace NetIdempo.Implementations.Services;

public class RequestForwarder(IContextReader reader,
    IRequestBuilder requestBuilder, IRequestExecutor requestExecutor) : IRequestForwarder
{
    public async Task ForwardRequestAsync(HttpContext context)
    {
        var serviceKey = reader.GetDestinationServiceFromHttpContext(context);
        var request = await requestBuilder.BuildRequest(context, serviceKey);
        await requestExecutor.ExecuteAsync(request, context);
    }
}