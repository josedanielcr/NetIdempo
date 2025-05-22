using Microsoft.AspNetCore.Http;
using NetIdempo.Abstractions.Core;

namespace NetIdempo.Implementations.Core;

public class RequestHandler : IRequestHandler
{
    public HttpContext HandleRequest(HttpContext context)
    {
        return context;
    }
}