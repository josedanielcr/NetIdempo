using Microsoft.AspNetCore.Http;

namespace NetIdempo.Abstractions.Core;

public interface IRequestHandler
{
    HttpContext HandleRequest(HttpContext context);
}