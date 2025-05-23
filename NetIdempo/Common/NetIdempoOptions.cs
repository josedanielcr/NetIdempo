namespace NetIdempo.Common;

public class NetIdempoOptions
{
    public string IdempotencyKeyHeader { get; set; } = string.Empty;
    public int IdempotencyKeyLifetime { get; set; }
    public Dictionary<string, ServiceConfig> Services { get; set; } = new();
}

public class ServiceConfig
{
    public string BaseUrl { get; set; } = string.Empty;
}