namespace NetIdempo.Common.Store;

public class IdempotencyCacheEntry
{
    public string Key { get; set; } = string.Empty;
    public byte[]? ResponseBody { get; set; } 
    public int? StatusCode { get; set; }
    public Dictionary<string, string[]>? Headers { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool IsCompleted { get; set; } = false;
}