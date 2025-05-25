using Microsoft.AspNetCore.Http;

namespace NetIdempo.Abstractions.Core;

public interface IRequestForwarder
{
    Task<HttpContext> ForwardRequestAsync(HttpContext context);
}