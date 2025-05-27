using Microsoft.AspNetCore.Http;

namespace NetIdempo.Abstractions.Helpers;

public interface IContextReader
{
    bool IsKeyPresent(HttpContext context);
    string GetServiceByPath(HttpContext context);
}