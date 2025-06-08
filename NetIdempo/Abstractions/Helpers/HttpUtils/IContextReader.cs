using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace NetIdempo.Abstractions.Helpers.HttpUtils;

public interface IContextReader
{
    bool IsIdempotencyKeyPresent(HttpContext context);
    string GetDestinationServiceFromHttpContext(HttpContext context);
    StringValues GetKeyFromHttpRequest(HttpContext context);
    string GetDestinationFinalUrlFromContext(HttpContext context, string serviceKey);
}