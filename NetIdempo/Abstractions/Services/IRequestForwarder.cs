using Microsoft.AspNetCore.Http;

namespace NetIdempo.Abstractions.Services;

public interface IRequestForwarder
{
    Task ForwardRequestAsync(HttpContext context);
}