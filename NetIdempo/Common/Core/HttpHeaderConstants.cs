namespace NetIdempo.Common.Core;

public static class HttpHeaderConstants
{
    public static readonly HashSet<string> RestrictedHeaders = new(StringComparer.OrdinalIgnoreCase)
    {
        "Host",
        "Content-Length",
        "Transfer-Encoding",
        "Connection",
        "Expect",
        "Keep-Alive"
    };
}