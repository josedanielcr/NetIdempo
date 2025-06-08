namespace NetIdempo.Abstractions.Helpers.Config;

public interface IOptionsReader
{
    string GetIdempotencyKeyHeader();
    string GetServiceKeyByIncomingRequestPath(string path);
    string GetDestinationServiceBaseUrlFromOptions(string serviceKey);
    string GetPathPrefixByServiceKey(string serviceKey);
}