using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NetIdempo.Abstractions.Core;
using NetIdempo.Abstractions.Helpers;
using NetIdempo.Common;

namespace NetIdempo.Implementations.Core;

public class RequestForwarder(IOptions<NetIdempoOptions> options, IContextReader reader) : IRequestForwarder
{
    public async Task<HttpContext> ForwardRequestAsync(HttpContext context)
    {
        var serviceKey = reader.GetServiceByPath(context);
        return null;
    }
}