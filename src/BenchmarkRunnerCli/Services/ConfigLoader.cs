using System.Text.Json;

namespace BenchmarkRunner.Utils;

public class LoadTestConfig
{
    public string ApiUrl { get; set; } = "";
    public string HttpMethod { get; set; } = "GET";
    public int ConcurrentRequests { get; set; } = 10;
    public int DurationSeconds { get; set; } = 10;
}

public static class ConfigLoader
{
    public static LoadTestConfig LoadConfig(string filePath)
    {
        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<LoadTestConfig>(json) 
               ?? throw new InvalidOperationException("Failed to deserialize config");
    }
}
