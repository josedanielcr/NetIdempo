using Microsoft.Extensions.Options;
using NetIdempo.Abstractions.Helpers.Config;
using NetIdempo.Common;

namespace NetIdempo.Implementations.Helpers.Config;

public class OptionsReader(IOptions<NetIdempoOptions> options) : IOptionsReader
{
    public string GetIdempotencyKeyHeader()
    {
        return options.Value.IdempotencyKeyHeader;
    }
    
    public string GetServiceKeyByIncomingRequestPath(string path)
    {
        foreach (var service in options.Value.Services.Where(service => path.Contains(service.Value.PathPrefix)))
        {
            return service.Key;
        }
        return string.Empty;
    }
    
    public string GetDestinationServiceBaseUrlFromOptions(string serviceKey)
    {
        return options.Value.Services.FirstOrDefault(service => service.Key == serviceKey).Value.BaseUrl;
    }
    
    public string GetPathPrefixByServiceKey(string serviceKey)
    {
        return options.Value.Services.FirstOrDefault(service => service.Key == serviceKey).Value.PathPrefix;
    }
}