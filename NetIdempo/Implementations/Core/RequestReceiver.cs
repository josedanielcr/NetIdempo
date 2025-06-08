using Microsoft.AspNetCore.Http;
using NetIdempo.Abstractions.Core;

namespace NetIdempo.Implementations.Core;

public class RequestReceiver(IRequestProcessor requestProcessor): IRequestReceiver
{
    public async Task ReceiveRequestAsync(HttpContext context)
    {
         await requestProcessor.ProcessRequestAsync(context);
    }
}