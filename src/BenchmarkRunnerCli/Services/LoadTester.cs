using System.Diagnostics;
using System.Net.Http;
using BenchmarkRunner.Models;

namespace BenchmarkRunner.Services;

public class LoadTester
{
    private readonly HttpClient _httpClient = new HttpClient();
    private readonly SqliteLogger _logger;

    public LoadTester(SqliteLogger logger)
    {
        _logger = logger;
    }

    public async Task RunLoadTestAsync(string url, string method, int concurrency, int durationSeconds)
    {
        Console.WriteLine($"Starting load test: {concurrency} concurrent requests for {durationSeconds} seconds...");

        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(durationSeconds));
        var tasks = new List<Task>();

        for (int i = 0; i < concurrency; i++)
        {
            tasks.Add(Task.Run(() => SendRequestsAsync(url, method, cts.Token)));
        }

        await Task.WhenAll(tasks);

        Console.WriteLine("Load test completed.");
    }

    private async Task SendRequestsAsync(string url, string method, CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();

                HttpResponseMessage? response = method.ToUpper() switch
                {
                    "GET" => await _httpClient.GetAsync(url, token),
                    "POST" => await _httpClient.PostAsync(url, null, token), // Simple POST with empty content
                    _ => throw new NotSupportedException($"HTTP method '{method}' is not supported")
                };

                stopwatch.Stop();

                var result = new ApiBenchmarkResult
                {
                    Endpoint = url,
                    LatencyMs = stopwatch.ElapsedMilliseconds,
                    StatusCode = (int)response.StatusCode,
                    Timestamp = DateTime.UtcNow
                };

                _logger.LogResult(result);
            }
            catch (TaskCanceledException)
            {
                // Test duration ended, exit gracefully
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
            }
        }
    }
}
