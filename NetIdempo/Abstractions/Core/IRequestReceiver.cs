using Microsoft.AspNetCore.Http;

namespace NetIdempo.Abstractions.Core;

public interface IRequestReceiver
{
    Task<HttpContext> ReceiveRequestAsync(HttpContext context);
}