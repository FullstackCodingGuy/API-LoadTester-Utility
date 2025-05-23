import pandas as pd
import sys
import subprocess



def summarize(csv_path):
    df = pd.read_csv(csv_path)

    # Take only essential fields for compact summary
    summary_df = df[['LatencyMs', 'StatusCode', 'Timestamp']].copy()

     # Take only the top 100 rows
    summary_df = summary_df.head(100)


    # Convert to string table format
    csv_string = summary_df.to_csv(index=False)

    prompt = f"""
You are a performance engineering assistant.

Analyze the following API benchmark result CSV and identify:
- Average, min, max latency, total requests, and error rate
- Any patterns in latency over time
- Any spikes or anomalies in response time
- Distribution of HTTP status codes
- Any insights or recommendations for improving performance

Here is the data:{csv_string}
Provide a professional performance analysis summary.
Identify any performance bottlenecks, anomalies, or areas for improvement based on the data provided. 
Provide actionable recommendations to optimize API performance.
"""

    result = subprocess.run(
        ["ollama", "run", "llama3.2", prompt],
        capture_output=True, text=True
    )
    print("\n=== Ollama Performance Summary ===")
    print(result.stdout)


if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("Usage: python summarize_results.py Results/results.csv")
    else:
        summarize(sys.argv[1])
