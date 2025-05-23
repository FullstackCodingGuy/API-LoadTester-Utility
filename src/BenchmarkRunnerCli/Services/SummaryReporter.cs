using BenchmarkRunner.Models;
using System.Data.SQLite;
using Dapper;

namespace BenchmarkRunner.Services;

public class SummaryReporter
{
    private readonly SQLiteConnection _connection;

    public SummaryReporter(string dbPath)
    {
        _connection = new SQLiteConnection($"Data Source={dbPath}");
        _connection.Open();
    }

    public void PrintSummary()
    {
        var results = _connection.Query<ApiBenchmarkResult>("SELECT * FROM Results").ToList();
        if (!results.Any())
        {
            Console.WriteLine("No results to summarize.");
            return;
        }

        var latencies = results.Select(r => r.LatencyMs).ToList();
        var statusGroups = results.GroupBy(r => r.StatusCode)
                                  .Select(g => new { StatusCode = g.Key, Count = g.Count() });

        Console.WriteLine("=== Benchmark Summary ===");
        Console.WriteLine($"Total Requests: {results.Count}");
        Console.WriteLine($"Avg Latency: {latencies.Average():F2} ms");
        Console.WriteLine($"Min Latency: {latencies.Min()} ms");
        Console.WriteLine($"Max Latency: {latencies.Max()} ms");
        Console.WriteLine();

        Console.WriteLine("Status Code Distribution:");
        foreach (var group in statusGroups)
        {
            Console.WriteLine($"  {group.StatusCode}: {group.Count} requests");
        }

        Console.WriteLine("==========================");
    }
}
