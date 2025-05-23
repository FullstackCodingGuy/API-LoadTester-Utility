using BenchmarkRunner.Services;
using BenchmarkRunner.Utils;

class Program
{
    static async Task<int> Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("Usage: BenchmarkRunner <config.json>");
            return 1;
        }

        string configPath = args[0];
        if (!File.Exists(configPath))
        {
            Console.WriteLine($"Config file not found: {configPath}");
            return 1;
        }

        var config = ConfigLoader.LoadConfig(configPath);
        using var logger = new SqliteLogger("benchmark_results.db");
        var loadTester = new LoadTester(logger);

        await loadTester.RunLoadTestAsync(
            config.ApiUrl,
            config.HttpMethod,
            config.ConcurrentRequests,
            config.DurationSeconds
        );

        Console.WriteLine("Results logged to benchmark_results.db");

        return 0;
    }
}
