using Microsoft.AspNetCore.Http;
using NetIdempo.Abstractions.Helpers.Body;

namespace NetIdempo.Implementations.Helpers.Body;

public class ResponseBodyCopier : IResponseBodyCopier
{
    public async Task CopyToHttpContextAsync(HttpResponseMessage response, HttpContext context)
    {
        if (response == null)
        {
            throw new ArgumentNullException(nameof(response), "Response cannot be null.");
        }
        var memoryStream = new MemoryStream();
        await response.Content.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        await memoryStream.CopyToAsync(context.Response.Body);
        memoryStream.Position = 0;
        response.Content = new StreamContent(memoryStream);
    }
}