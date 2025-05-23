using BenchmarkRunner.Models;
using System.Data.SQLite;
using Dapper;
using System.Globalization;

namespace BenchmarkRunner.Reports;

public static class CsvExporter
{
    public static void Export(string dbPath, string csvPath)
    {
        using var connection = new SQLiteConnection($"Data Source={dbPath}");
        var results = connection.Query<ApiBenchmarkResult>("SELECT * FROM Results");

        using var writer = new StreamWriter(csvPath);
        writer.WriteLine("Endpoint,LatencyMs,StatusCode,Timestamp");

        foreach (var result in results)
        {
            writer.WriteLine($"\"{result.Endpoint}\",{result.LatencyMs},{result.StatusCode},{result.Timestamp.ToString("o", CultureInfo.InvariantCulture)}");
        }

        Console.WriteLine($"Results exported to: {csvPath}");
    }
}
