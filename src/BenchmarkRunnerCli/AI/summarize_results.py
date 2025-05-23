import openai
import os
import pandas as pd
import sys

import subprocess

def ollama_summarize(prompt):
    result = subprocess.run(
        ["ollama", "run", "llama3", prompt],
        capture_output=True, text=True
    )
    print("\n=== Ollama Summary ===")
    print(result.stdout)



# this function uses openAi to summarize the results
def summarize(csv_path):
    df = pd.read_csv(csv_path)

    avg_latency = df['LatencyMs'].mean()
    min_latency = df['LatencyMs'].min()
    max_latency = df['LatencyMs'].max()
    total_requests = len(df)

    status_counts = df['StatusCode'].value_counts().to_dict()

    prompt = f"""
    You are an expert performance engineer. Here is an API benchmark summary:
    - Total requests: {total_requests}
    - Average latency: {avg_latency:.2f} ms
    - Min latency: {min_latency} ms
    - Max latency: {max_latency} ms
    - Status code breakdown: {status_counts}

    Provide a professional summary of the test performance.
    """

    openai.api_key = os.getenv("OPENAI_API_KEY")  # Set in environment
    response = openai.ChatCompletion.create(
        model="gpt-4",  # or gpt-3.5-turbo
        messages=[{"role": "user", "content": prompt}],
        temperature=0.7
    )

    print("\n=== AI Summary ===")
    print(response['choices'][0]['message']['content'])

if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("Usage: python summarize_results.py <results.csv>")
    else:
        ollama_summarize(sys.argv[1])
