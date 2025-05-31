using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace NetIdempo.Abstractions.Helpers;

public interface IContextReader
{
    bool IsIdempotencyKeyPresent(HttpContext context);
    string GetServiceByPath(HttpContext context);
    StringValues GetKeyFromHttpRequest(HttpContext context);
}