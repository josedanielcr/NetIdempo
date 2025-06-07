using System.Diagnostics;
using System.Net.Http.Headers;

const string gatewayBaseUrl = "http://localhost:5037"; // API Gateway using NetIdempo
const string idempotencyHeader = "Idempotency-Key";
const string jsonPayload = """{ "date": "2025-06-04", "temperatureC": 25, "summary": "Warm" }""";

string[] pathPrefixes = { "testservice1", "testservice2" };
string[] idempotencyKeys = { "key-1", "key-2", "key-1", "", null };

var httpClient = new HttpClient();
var rand = new Random();

for (var i = 0; i < 10; i++) // send 10 test requests
{
    var path = pathPrefixes[rand.Next(pathPrefixes.Length)];
    string? key = idempotencyKeys[rand.Next(idempotencyKeys.Length)];

    var url = $"{gatewayBaseUrl}/{path}/weatherforecast";
    Console.WriteLine($"\n[#{i + 1}] Sending to: {url} | Idempotency-Key: {key ?? "none"}");

    var request = new HttpRequestMessage(HttpMethod.Post, url)
    {
        Content = new StringContent(jsonPayload)
    };
    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

    if (!string.IsNullOrEmpty(key))
        request.Headers.Add(idempotencyHeader, key);

    var sw = Stopwatch.StartNew();
    var response = await httpClient.SendAsync(request);
    sw.Stop();

    var body = await response.Content.ReadAsStringAsync();
    Console.WriteLine($"Status: {response.StatusCode}, Time: {sw.ElapsedMilliseconds} ms");
    Console.WriteLine($"Response body: {body}");
}