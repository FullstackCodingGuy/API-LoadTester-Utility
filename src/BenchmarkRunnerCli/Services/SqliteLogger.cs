using System;
using System.Data.SQLite;
using Dapper;
using BenchmarkRunner.Models;

namespace BenchmarkRunner.Services
{
    public class SqliteLogger : IDisposable
    {
        private readonly SQLiteConnection _connection;

        public SqliteLogger(string databaseFile = "benchmark.db")
        {
            _connection = new SQLiteConnection($"Data Source={databaseFile};Version=3;");
            _connection.Open();
            CreateTableIfNotExists();
        }

        private void CreateTableIfNotExists()
        {
            var createTableSql = @"
                CREATE TABLE IF NOT EXISTS Results (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Endpoint TEXT NOT NULL,
                    LatencyMs INTEGER NOT NULL,
                    StatusCode INTEGER NOT NULL,
                    Timestamp TEXT NOT NULL
                );";

            _connection.Execute(createTableSql);
        }

        public void LogResult(ApiBenchmarkResult result)
        {
            var insertSql = @"
                INSERT INTO Results (Endpoint, LatencyMs, StatusCode, Timestamp)
                VALUES (@Endpoint, @LatencyMs, @StatusCode, @Timestamp);";

            _connection.Execute(insertSql, new
            {
                Endpoint = result.Endpoint,
                LatencyMs = result.LatencyMs,
                StatusCode = result.StatusCode,
                Timestamp = result.Timestamp.ToString("o") // ISO8601 format
            });
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
