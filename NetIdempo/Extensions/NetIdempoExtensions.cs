using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetIdempo.Abstractions.Core;
using NetIdempo.Abstractions.Helpers;
using NetIdempo.Common;
using NetIdempo.Implementations.Core;
using NetIdempo.Implementations.Helpers;

namespace NetIdempo.Extensions;

public static class NetIdempoExtensions
{
    private const string DefaultConfigurationSectionName = "NetIdempo";
    
    public static IServiceCollection AddNetIdempo(this IServiceCollection services,
        IConfiguration configuration)
    {
        AddConfiguration(services, configuration);
        AddHelperMethods(services);
        AddCoreImplementations(services);
        return services;
    }

    private static void AddConfiguration(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<NetIdempoOptions>(configuration.GetSection(DefaultConfigurationSectionName));
    }

    private static void AddHelperMethods(IServiceCollection services)
    {
        services.AddSingleton<IContextReader, ContextReader>();
    }

    private static void AddCoreImplementations(IServiceCollection services)
    {
        services.AddSingleton<IRequestReceiver, RequestReceiver>();
        services.AddSingleton<IRequestProcessor, RequestProcessor>();
        services.AddSingleton<IRequestForwarder, RequestForwarder>();
        services.AddSingleton<IIdempotencyStore, IIdempotencyStore>();
    }
}