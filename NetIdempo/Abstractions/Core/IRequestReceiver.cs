using Microsoft.AspNetCore.Http;

namespace NetIdempo.Abstractions.Core;

public interface IRequestReceiver
{
    Task ReceiveRequestAsync(HttpContext context);
}