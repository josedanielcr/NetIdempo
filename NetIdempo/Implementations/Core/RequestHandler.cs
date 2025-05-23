using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NetIdempo.Abstractions.Core;
using NetIdempo.Common;

namespace NetIdempo.Implementations.Core;

public class RequestHandler(IOptions<NetIdempoOptions> options) : IRequestHandler
{
    public HttpContext HandleRequest(HttpContext context)
    {
        return context;
    }
}