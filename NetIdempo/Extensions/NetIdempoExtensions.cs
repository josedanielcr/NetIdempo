using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetIdempo.Abstractions.Core;
using NetIdempo.Implementations.Core;

namespace NetIdempo.Extensions;

public static class NetIdempoExtensions
{
    public static IServiceCollection AddNetIdempo(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IRequestHandler, RequestHandler>();
        return services;
    }
}