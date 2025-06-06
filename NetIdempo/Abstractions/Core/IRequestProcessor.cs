using Microsoft.AspNetCore.Http;

namespace NetIdempo.Abstractions.Core;

public interface IRequestProcessor
{
    Task ProcessRequestAsync(HttpContext context);
}