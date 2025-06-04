using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetIdempo.Abstractions.Core;
using NetIdempo.Abstractions.Helpers;
using NetIdempo.Abstractions.Helpers.Body;
using NetIdempo.Abstractions.Helpers.Config;
using NetIdempo.Abstractions.Helpers.Headers;
using NetIdempo.Abstractions.Helpers.HttpUtils;
using NetIdempo.Abstractions.Services;
using NetIdempo.Abstractions.Store;
using NetIdempo.Common;
using NetIdempo.Implementations.Core;
using NetIdempo.Implementations.Helpers.Body;
using NetIdempo.Implementations.Helpers.Config;
using NetIdempo.Implementations.Helpers.Headers;
using NetIdempo.Implementations.Helpers.HttpUtils;
using NetIdempo.Implementations.Services;
using NetIdempo.Implementations.Store;

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
        services.AddScoped<IOptionsReader, OptionsReader>();
        services.AddScoped<IRequestBodyCopier, RequestBodyCopier>();
        services.AddScoped<IResponseBodyCopier, ResponseBodyCopier>();
        services.AddScoped<ICacheBodyCopier, CacheBodyCopier>();
        services.AddScoped<ICacheHeaderCopier, CacheHeaderCopier>();
        services.AddScoped<IRequestHeaderCopier, RequestHeaderCopier>();
        services.AddScoped<IResponseHeaderCopier, ResponseHeaderCopier>();
        services.AddScoped<IHttpResponseCopier, HttpResponseCopier>();
        services.AddScoped<IHttpRequestBuilder, HttpRequestBuilder>();
    }

    private static void AddCoreImplementations(IServiceCollection services)
    {
        services.AddScoped<IRequestReceiver, RequestReceiver>();
        services.AddScoped<IRequestProcessor, RequestProcessor>();
        services.AddScoped<IRequestForwarder, RequestForwarder>();
        services.AddScoped<IIdempotencyStore, IdempotencyStore>();
        services.AddScoped<IRequestBuilder, RequestBuilder>();
        services.AddScoped<IRequestExecutor, RequestExecutor>();
    }
    
    public static IApplicationBuilder UseNetIdempo(this IApplicationBuilder app)
    {
        return app.UseMiddleware<NetIdempoMiddleware>();
    }
}