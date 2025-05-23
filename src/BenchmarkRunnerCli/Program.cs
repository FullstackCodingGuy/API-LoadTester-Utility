using BenchmarkRunner.Services;
using BenchmarkRunner.Utils;
using BenchmarkRunner.Reports;

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
        const string dbPath = "benchmark_results.db";

        using var logger = new SqliteLogger(dbPath);
        var loadTester = new LoadTester(logger);

        await loadTester.RunLoadTestAsync(
            config.ApiUrl,
            config.HttpMethod,
            config.ConcurrentRequests,
            config.DurationSeconds
        );

        // Phase 2 Reporting
        var reporter = new SummaryReporter(dbPath);
        reporter.PrintSummary();

        CsvExporter.Export(dbPath, "Results/results.csv");

        return 0;
    }
}
