using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NetIdempo.Common;

namespace NetIdempo.Abstractions.Helpers;

public interface IContextReader
{
    bool IsKeyPresent(HttpContext? context);
    string GetServiceByPath(HttpContext context);
    
}