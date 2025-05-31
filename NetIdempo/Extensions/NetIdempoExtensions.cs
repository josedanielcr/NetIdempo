using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetIdempo.Abstractions.Core;
using NetIdempo.Abstractions.Helpers;
using NetIdempo.Abstractions.Services;
using NetIdempo.Abstractions.Store;
using NetIdempo.Common;
using NetIdempo.Implementations.Core;
using NetIdempo.Implementations.Helpers;
using NetIdempo.Implementations.Services;

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
        services.AddHttpClient<IRequestExecutor, RequestExecutor>(DefaultConfigurationSectionName, client =>
        {
            client.Timeout = TimeSpan.FromSeconds(configuration.GetSection("NetIdempo:ServiceTimeoutSeconds").Get<int>());
        });
    }

    private static void AddHelperMethods(IServiceCollection services)
    {
        services.AddScoped<IContextReader, ContextReader>();
    }

    private static void AddCoreImplementations(IServiceCollection services)
    {
        services.AddScoped<IRequestReceiver, RequestReceiver>();
        services.AddScoped<IRequestProcessor, RequestProcessor>();
        services.AddScoped<IRequestForwarder, RequestForwarder>();
        services.AddScoped<IIdempotencyStore, IIdempotencyStore>();
        services.AddScoped<IRequestBuilder, RequestBuilder>();
        services.AddScoped<IRequestExecutor, RequestExecutor>();
    }
}