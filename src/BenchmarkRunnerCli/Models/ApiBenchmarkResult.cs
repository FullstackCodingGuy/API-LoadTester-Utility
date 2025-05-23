namespace BenchmarkRunner.Models
{
    public class ApiBenchmarkResult
    {
        public string Endpoint { get; set; }
        public long LatencyMs { get; set; }
        public int StatusCode { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
